using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Database;
using NoPersaService.Services;
using NoPersaService.Models;
using NoPersaService.Util;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace NoPersa.Tests.MaintenanceTests
{
    [TestClass]
    public class DeliverDailyTest : ITest
    {
        private NoPersaDbContext context;
        private DailyDeliveryService service;
        private IServiceProvider serviceProvider;
        private ILogger<DailyDeliveryService> logger;

        [TestInitialize]
        public void Setup()
        {
            DotNetEnv.Env.Load(@"..\..\..\..\.env");

            var services = new ServiceCollection();
            ProgramBuilder.RegisterAutoMapperProfiles(services);
            ProgramBuilder.RegisterFluentValidations(services);
            services.AddDbContext<NoPersaDbContext>(opt => opt.UseSqlite("DataSource=:memory:"));
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            context = serviceProvider.GetRequiredService<NoPersaDbContext>();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            logger = new Mock<ILogger<DailyDeliveryService>>().Object;

            var httpClient = new HttpClient();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(NoPersaDbContext)))
                               .Returns(context);


            serviceProvider = serviceProviderMock.Object;

            service = new DailyDeliveryService(serviceProvider, logger, httpClient);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.MonthlyOverviews.AddRange(StaticMonthlyOverview.GetMonthlyOverviews(DateTime.Today.Year, DateTime.Today.Month));
            context.SaveChanges();

            var dailyOverviews1 = StaticDailyOverviews.GetDailyOverview1(context.MonthlyOverviews.First(m => m.Id == 1));
            var dailyOverviews2 = StaticDailyOverviews.GetDailyOverview2(context.MonthlyOverviews.First(m => m.Id == 2));
            context.DailyOverviews.AddRange(dailyOverviews1);
            context.DailyOverviews.AddRange(dailyOverviews2);

            context.Holidays.AddRange(StaticHolidays.GetHolidays());
            context.Maintenances.AddRange(StaticMaintenances.GetMaintenances());

            context.SaveChanges();
        }

       
        [TestMethod]
        [TestOrder(1)]
        public async Task SetDailyDelivery()
        { 
            await service.CatchUp(context, (await context.Maintenances.AsNoTracking().FirstOrDefaultAsync(m => m.Id == 1))!);

            ICollection<DailyOverview> dailyOverviews1 = (await context.Customers.Include(c => c.MonthlyOverviews).ThenInclude(d => d.DailyOverviews).FirstOrDefaultAsync(c => c.Id == 1))!.MonthlyOverviews.FirstOrDefault(d => d.Id == 1)!.DailyOverviews; 
            ICollection<DailyOverview> dailyOverviews2 = (await context.Customers.Include(c => c.MonthlyOverviews).ThenInclude(d => d.DailyOverviews).FirstOrDefaultAsync(c => c.Id == 2))!.MonthlyOverviews.FirstOrDefault(d => d.Id == 2)!.DailyOverviews;

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
