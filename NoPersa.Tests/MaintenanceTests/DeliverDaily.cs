using MaintenanceService.Database;
using MaintenanceService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using SharedLibrary.Models;

namespace NoPersa.Tests.MaintenanceTests
{
    [TestClass]
    public class DeliverDaily : ITest
    {
        private NoPersaDbContext context;
        private DailyDelivery service;
        private IServiceProvider serviceProvider;
        private ILogger<DailyDelivery> logger;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NoPersaDbContext>()
            .UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging()
            .Options;

            context = new NoPersaDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            logger = new Mock<ILogger<DailyDelivery>>().Object;

            var httpClient = new HttpClient();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(NoPersaDbContext)))
                               .Returns(context);


            serviceProvider = serviceProviderMock.Object;

            service = new DailyDelivery(serviceProvider, logger, httpClient);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.MonthlyOverview.AddRange(StaticMonthlyOverview.GetMonthlyOverviews(DateTime.Today.Year, DateTime.Today.Month));
            context.SaveChanges();

            var dailyOverviews1 = StaticDailyOverviews.GetDailyOverview1(context.MonthlyOverview.First(m => m.Id == 1));
            var dailyOverviews2 = StaticDailyOverviews.GetDailyOverview2(context.MonthlyOverview.First(m => m.Id == 2));
            context.DailyOverview.AddRange(dailyOverviews1);
            context.DailyOverview.AddRange(dailyOverviews2);

            context.Holiday.AddRange(StaticHolidays.GetHolidays());
            context.Maintenance.AddRange(StaticMaintenances.GetMaintenances());

            context.SaveChanges();
        }

       
        [TestMethod]
        [TestOrder(1)]
        public async Task SetDailyDelivery()
        { 
            await service.CatchUp(context, (await context.Maintenance.AsNoTracking().FirstOrDefaultAsync(m => m.Id == 1))!);

            ICollection<DailyOverview> dailyOverviews1 = (await context.Customer.Include(c => c.MonthlyOverviews).ThenInclude(d => d.DailyOverviews).FirstOrDefaultAsync(c => c.Id == 1))!.MonthlyOverviews.FirstOrDefault(d => d.Id == 1)!.DailyOverviews; 
            ICollection<DailyOverview> dailyOverviews2 = (await context.Customer.Include(c => c.MonthlyOverviews).ThenInclude(d => d.DailyOverviews).FirstOrDefaultAsync(c => c.Id == 2))!.MonthlyOverviews.FirstOrDefault(d => d.Id == 2)!.DailyOverviews;

            Assert.AreEqual(1, dailyOverviews1.FirstOrDefault(d => d.DayOfMonth == 1)!.NumberOfBoxes);
            Assert.AreEqual(10.5, dailyOverviews1.FirstOrDefault(d => d.DayOfMonth == 1)!.Price);
            Assert.AreEqual(0, dailyOverviews2.FirstOrDefault(d => d.DayOfMonth == 1)!.NumberOfBoxes);
            Assert.AreEqual(0, dailyOverviews2.FirstOrDefault(d => d.DayOfMonth == 1)!.Price);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.CloseConnection();
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
