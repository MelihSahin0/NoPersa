using AutoMapper;
using DeliveryService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Holiday = SharedLibrary.Models.Holiday;
using Route = SharedLibrary.Models.Route;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryManagmentController : ControllerBase
    {
        private NoPersaDbContext context;
        private readonly HttpClient httpClient;
        private readonly ILogger<DeliveryManagmentController> logger;
        private readonly IMapper mapper;
        private readonly string currentCountry = "at"; //TODO: Will be removed in the future

        public DeliveryManagmentController(ILogger<DeliveryManagmentController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper, HttpClient httpClient)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.httpClient = httpClient;
        }

        [HttpGet("GetRoutesOverview", Name = "GetRoutesOverview")]
        public IActionResult GetRoutesOverview()
        {
            try
            {
                DTORoutes dTORoutes = new();
                List<DTORouteOverview> routes = [];

                foreach (Route dbRoute in context.Routes.Include(r => r.Customers))
                {
                    routes.Add(mapper.Map<DTORouteOverview>(dbRoute));
                }
                dTORoutes.RouteOverview = [.. routes];

                return Ok(dTORoutes);
            }
            catch (ValidationException e)
            {
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest("An error occurred while processing your request.");
            }
        }


        [HttpPost("UpdateRoutes", Name = "UpdateRoutes")]
        public IActionResult UpdateRoutes([FromBody] DTORoutes routes)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<Route> newRoutes = [];
                List<Route> oldRoutes = [];
                foreach (var routeDto in routes.RouteOverview ?? [])
                {
                    Route route = mapper.Map<Route>(routeDto);
                    route.Customers = [];

                    if (route.Id == 0)
                    {
                        newRoutes.Add(route);
                    }
                    else
                    {
                        oldRoutes.Add(route);
                    }
                }

                var dbExistingRoutes = context.Routes.Where(r => oldRoutes.Select(or => or.Id).Contains(r.Id)).ToList();
                foreach (var dbExistingRoute in dbExistingRoutes)
                {
                    var dbUpdatedRoute = oldRoutes.FirstOrDefault(or => or.Id == dbExistingRoute.Id);
                    if (dbUpdatedRoute != null)
                    {
                        dbExistingRoute.Position = dbUpdatedRoute.Position;
                        dbExistingRoute.Name = dbUpdatedRoute.Name;
                    }
                }

                var dbNotFoundRoutes = context.Routes.Where(er => !oldRoutes.Select(or => or.Id).Contains(er.Id)).Include(r => r.Customers).ToList();
                foreach (var dbObsoleteRoute in dbNotFoundRoutes)
                {
                    foreach (Customer dbCustomer in dbObsoleteRoute.Customers)
                    {
                        dbCustomer.Position = null;
                    }
                    context.Routes.Remove(dbObsoleteRoute);
                }

                context.Routes.AddRange(newRoutes);

                context.SaveChanges();
                transaction.Commit();

                return Ok(routes);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e.Message);
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetDeliveryStatus", Name = "GetDeliveryStatus")]
        public async Task<IActionResult> GetDeliveryStatus([FromBody] DTOSelectedDay dTOSelectedDay)
        {
            try
            {
                await CheckHolidays(dTOSelectedDay);

                DTODeliveryStatus dTODeliveryStatus = new();
                List<DTORouteDetails> routes = [];

                List<Route> dbRoutes = await context.Routes.Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                          .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                          .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month))
                                                                    .ThenInclude(d=>d.DailyOverviews.Where(x => x.DayOfMonth == dTOSelectedDay.Day))
                                          .ToListAsync();

                foreach (Route dbRoute in dbRoutes)
                {
                    List<DTOCustomerRoute> customerRoutes = [];
                    foreach (Customer dbCustomer in dbRoute.Customers ?? [])
                    {
                        DTOCustomerRoute dTOCustomerRoutes = mapper.Map<DTOCustomerRoute>(dbCustomer);

                        MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x =>
                                                                                                      x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month);

                        if (dbFoundOverview != null)
                        {
                            int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
                            dTOCustomerRoutes.ToDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && SetDeliveryToTrueOrFalse(dbCustomer, dTOSelectedDay));
                        }
                        else
                        {
                            dTOCustomerRoutes.ToDeliver = SetDeliveryToTrueOrFalse(dbCustomer, dTOSelectedDay);
                        }

                        customerRoutes.Add(dTOCustomerRoutes);
                    }
                    DTORouteDetails dTORouteDetails = mapper.Map<DTORouteDetails>(dbRoute);
                    dTORouteDetails.CustomersRoute = [..customerRoutes];
                    routes.Add(dTORouteDetails);
                }

                dTODeliveryStatus.RouteDetails = [.. routes];

                return Ok(dTODeliveryStatus);
            }
            catch (ValidationException e)
            {
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetRouteDetails", Name = "GetRouteDetails")]
        public IActionResult GetRouteDetails([FromBody] DTOSelectedDay dTOSelectedDay)
        {
            try
            {
                return Ok();
            }
            catch (ValidationException e)
            {
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest("An error occurred while processing your request.");
            }
        }


        private bool SetDeliveryToTrueOrFalse(Customer dbCustomer, DTOSelectedDay dTOSelectedDay)
        {
            DateTime date = new(dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day);
            string propertyName = date.DayOfWeek.ToString();

            if (dbCustomer.TemporaryDelivery)
            {
                return true;
            }
            if (dbCustomer.TemporaryNoDelivery)
            {
                return false;
            }

            Holiday? holiday = context.Holidays.FirstOrDefault(h => h.Country.Equals(currentCountry) && h.Year == dTOSelectedDay.Year && h.Month == dTOSelectedDay.Month && h.Day == dTOSelectedDay.Day);

            if (holiday == null)
            {
                var propertyInfo = typeof(Weekday).GetProperty(propertyName);

                if (propertyInfo == null || !propertyInfo.CanRead)
                {
                    throw new InvalidOperationException($"Property '{propertyName}' does not exist or cannot be read.");
                }

                return (bool)(propertyInfo.GetValue(dbCustomer.Workdays) ?? true);  
            }
            else
            {
                var propertyInfo = typeof(Weekday).GetProperty(propertyName);

                if (propertyInfo == null || !propertyInfo.CanRead)
                {
                    throw new InvalidOperationException($"Property '{propertyName}' does not exist or cannot be read.");
                }

                return (bool)(propertyInfo.GetValue(dbCustomer.Holidays) ?? true);
            }
        }

        private async Task CheckHolidays(DTOSelectedDay dTOSelectedDay)
        {
            if (!context.Holidays.Any(h => h.Country.Equals(currentCountry) && h.Year == dTOSelectedDay.Year))
            {
                using var transaction = await context.Database.BeginTransactionAsync();

                var response = await httpClient.GetAsync($"https://calendarific.com/api/v2/holidays?&api_key={Environment.GetEnvironmentVariable("HOLIDAY_API")}&country={currentCountry}&year={dTOSelectedDay.Year}");
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
                        await context.Holidays.AddRangeAsync(holidays);
                        await context.SaveChangesAsync();
                    }
                }
                await transaction.CommitAsync();
            }
        }
    }
}
