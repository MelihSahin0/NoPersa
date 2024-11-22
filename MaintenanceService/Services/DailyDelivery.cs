using MaintenanceService.Database;
using SharedLibrary.Models;
using System.Text.Json;
using EFCore.BulkExtensions;
using Holiday = SharedLibrary.Models.Holiday;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Util;
using SharedLibrary.DTOs.Maintenance;

namespace MaintenanceService.Services
{
    public class DailyDelivery : IHostedService, IDisposable
    {
        private Timer? timer;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DailyDelivery> logger;
        private readonly HttpClient httpClient;
        private readonly string currentCountry = "at"; //TODO: Will be removed in the future

        public DailyDelivery(IServiceProvider serviceProvider, ILogger<DailyDelivery> logger, HttpClient httpClient)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var now =  DateTime.Now;
            var targetTime = DateTime.Today.AddHours(18); //When to update the DailyOverviews

            var initialDelay = targetTime - now;
            if (initialDelay < TimeSpan.Zero)
            {
                initialDelay = TimeSpan.Zero;
            }

            timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        public async Task CatchUp(NoPersaDbContext context, Maintenance maintenance)
        {
            //HOLIDAY
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await CheckHolidays(context, DateTime.Today.Year);

                Random random = new();
                double randomDay = random.NextDouble() * 364 + 1;
                if (randomDay < DateTime.Today.DayOfYear)
                {
                    await CheckHolidays(context, DateTime.Today.AddYears(1).Year);
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                logger.LogError(e.Message);
            }

            //Save Deliveries
            using var transaction2 = await context.Database.BeginTransactionAsync();
            try
            {
                DateTime lowestDate = maintenance.Date;

                List<Customer> dbCustomers = await context.Customer.Where(r => r.RouteId != int.MinValue || (r.RouteId == int.MinValue && r.MonthlyOverviews.Any(x => x.Year == maintenance.Date.Year && x.Month == maintenance.Date.Month)))
                                                                   .Include(w => w.Workdays)
                                                                   .Include(h => h.Holidays)
                                                                   .Include(m => m.MonthlyOverviews.Where(x => x.Year > maintenance.Date.Year || (x.Year == maintenance.Date.Year && x.Month >= maintenance.Date.Month)))
                                                                       .ThenInclude(d => d.DailyOverviews)
                                                                   .Include(a => a.Article)
                                                                   .ToListAsync();

                DateTime time = new(maintenance.Date.Year, maintenance.Date.Month, maintenance.Date.Day);
                while (DateTime.Today.Date >= time.Date)
                {
                    Holiday? holiday = await context.Holiday.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == time.Year && h.Month == time.Month && h.Day == time.Day);

                    foreach (Customer dbCustomer in dbCustomers)
                    {
                        MonthlyOverview? dbMonthlyOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == time.Year && x.Month == time.Month);

                        if (dbMonthlyOverview == null)
                        {
                            if (dbCustomer.RouteId != int.MinValue)
                            {
                                CheckMonthlyOverview.CheckAndAdd(dbCustomer, time);
                                dbMonthlyOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == time.Year && x.Month == time.Month);
                            }
                        }

                        if (dbMonthlyOverview != null)
                        {
                            DailyOverview dbDailyOverview = dbMonthlyOverview!.DailyOverviews.First(x => x.DayOfMonth == time.Day);
                            if (dbDailyOverview.NumberOfBoxes == null)
                            {
                                if (CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, time.Year, time.Month, time.Day)
                                    && dbCustomer.RouteId != int.MinValue)
                                {
                                    dbDailyOverview.NumberOfBoxes = dbCustomer.DefaultNumberOfBoxes;
                                    dbDailyOverview.Price = dbCustomer.Article.Price;
                                }
                                else
                                {
                                    dbDailyOverview.NumberOfBoxes = 0;
                                    dbDailyOverview.Price = 0;
                                }
                            }
                            else if (dbDailyOverview.Price == null)
                            {
                                dbDailyOverview.Price = dbCustomer.Article.Price;
                            }
                        }
                    }
                    time = time.AddDays(1);
                }

                const int batchSize = 1000;
                for (int i = 0; i < dbCustomers.Count; i += batchSize)
                {
                    var batch = dbCustomers.Skip(i).Take(batchSize).ToList();
                    await context.BulkInsertOrUpdateAsync(batch);
                }

                (await context.Maintenance.FirstAsync(m => m.Id == 1)).Date = DateTime.Today.AddDays(1).Date;
                await context.SaveChangesAsync();

                await transaction2.CommitAsync();
            }
            catch (Exception e)
            {
                transaction2.Rollback();
                logger.LogError(e, "DailyDelivery daily save failed");
            }
        }

        private void DoWork(object? state)
        {
            Task.Run(async () =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<NoPersaDbContext>();

                    var maintenance = await context.Maintenance.FirstOrDefaultAsync(m => m.Type == MaintenanceTypes.DailyDelivery.ToString());
                    if (maintenance != null)
                    {
                        if (DateTime.Today.Date >= maintenance.Date)
                        {
                            await CatchUp(context, maintenance);
                        }
                    }
                    else
                    {
                        //Do it so everything works again
                        using var transaction = await context.Database.BeginTransactionAsync();
                        try
                        {
                            await context.Maintenance.AddAsync(new Maintenance() { Id = 1, Type = MaintenanceTypes.DailyDelivery.ToString(), Date = DateTime.Today.Date });
                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            logger.LogError(ex, "An error occurred while processing maintenance and updating articles.");
                        }
                    }
                }

            }).ConfigureAwait(true);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
            httpClient.Dispose();
        }

        private async Task CheckHolidays(NoPersaDbContext context, int year)
        {
            if (!await context.Holiday.AsNoTracking().AnyAsync(h => h.Country.Equals(currentCountry) && h.Year == year))
            {
                var response = await httpClient.GetAsync($"https://calendarific.com/api/v2/holidays?&api_key={Environment.GetEnvironmentVariable("HOLIDAY_API")}&country={currentCountry}&year={year}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DTOYearlyHolidays? apiResponse = JsonSerializer.Deserialize<DTOYearlyHolidays>(await response.Content.ReadAsStringAsync());

                    if (apiResponse != null)
                    {
                        List<Holiday> holidays = [];
                        foreach (var holiday in apiResponse.Response?.Holidays ?? [])
                        {
                            if (holiday.Type.Contains("National holiday"))
                                holidays.Add(new()
                                {
                                    Country = currentCountry,
                                    Year = holiday.Date.Datetime.Year,
                                    Month = holiday.Date.Datetime.Month,
                                    Day = holiday.Date.Datetime.Day,
                                });
                        }
                        await context.Holiday.AddRangeAsync(holidays);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
