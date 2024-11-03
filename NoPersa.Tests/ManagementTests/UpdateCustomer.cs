using AutoMapper;
using EFCore.BulkExtensions;
using ManagementService.Controllers;
using ManagementService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using SharedLibrary.DTOs;
using SharedLibrary.MappingProfiles;
using SharedLibrary.Models;

namespace NoPersa.Tests.ManagementTests
{
    [TestClass]
    public class UpdateCustomer : ITest
    {
        private NoPersaDbContext context;
        private CustomerManagementController controller;
        private IMapper mapper;
        private ILogger<CustomerManagementController> logger;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NoPersaDbContext>()
            .UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging()
            .Options;

            context = new NoPersaDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomerProfile>();
                cfg.AddProfile<DailyOverviewProfile>();
                cfg.AddProfile<MonthlyOverviewProfile>();
                cfg.AddProfile<WeekdaysProfile>();
                cfg.AddProfile<RouteProfile>();
                cfg.AddProfile<DefaultProfile>();
                cfg.AddProfile<BoxConfigurationProfile>();
            });

            mapper = config.CreateMapper();
            logger = new Mock<ILogger<CustomerManagementController>>().Object;
            controller = new CustomerManagementController(logger, context, mapper);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.MonthlyOverviews.AddRange(StaticMonthlyOverview.GetMonthlyOverviews(DateTime.Today.Year, DateTime.Today.Month));
            context.SaveChanges();

            var dailyOverviews = StaticDailyOverviews.GetDailyOverview1(context.MonthlyOverviews.First(m => m.Id == 1));
            context.DailyOverviews.AddRange(dailyOverviews);
            context.SaveChanges();

            context.LightDiets.AddRange(StaticLightDiets.GetLightDiets());
            context.BoxContents.AddRange(StaticBoxContents.GetBoxContents());
            context.PortionSizes.AddRange(StaticPortionSizes.GetPortionSizes());
            context.SaveChanges();

            List<CustomersLightDiet> customersLightDiets = [];
            customersLightDiets.AddRange(StaticCustomersLightDiets.GetCustomersLightDiets());
            context.BulkInsert(customersLightDiets);

            List<CustomersMenuPlan> customersMenuPlans = [];
            customersMenuPlans.AddRange(StaticCustomersMenuPlan.GetCustomersMenuPlan());
            context.BulkInsert(customersMenuPlans);
        }

        [TestMethod]
        [TestOrder(1)]
        public void UpdateCustomers()
        {
            List<Customer> customers = [.. context.Customers.AsNoTracking().Include(c => c.MonthlyOverviews).ThenInclude(m => m.DailyOverviews)
                                                                           .Include(w => w.Workdays).Include(h => h.Holidays)];
            Customer customer = customers.FirstOrDefault(c => c.Id == 1)!;
            customer.Name = "Customer 0";
                
            controller.UpdateCustomer(mapper.Map<DTOCustomer>(customer));
            Customer dbCustomer = context.Customers.FirstOrDefault(c => c.Id == 1)!;

            Assert.AreEqual(customer.Name, dbCustomer.Name);
        }

        [TestMethod]
        [TestOrder(1)]
        public void InsertCustomers()
        {
            List<Customer> customers = [.. context.Customers.AsNoTracking().Include(c => c.MonthlyOverviews).ThenInclude(m => m.DailyOverviews)
                                                                           .Include(w => w.Workdays).Include(h => h.Holidays)
                                                                           .Include(cld => cld.CustomersLightDiets)
                                                                           .Include(cmp => cmp.CustomerMenuPlans)];

            Customer customer = customers.FirstOrDefault(c => c.Id == 1)!;
            customer.Name = "Customer new";
            customer.Id = 0;

            List<LightDiet> lightDiets = [.. context.LightDiets.AsNoTracking()];
            List<BoxContent> boxContents = [.. context.BoxContents.AsNoTracking()];
            List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];

            controller.InsertCustomer(mapper.Map<DTOCustomer>(customer));
            Customer dbCustomer = context.Customers.FirstOrDefault(c => c.Name == "Customer new")!;

            Assert.AreEqual(customer.Name, dbCustomer.Name);
            Assert.AreEqual(lightDiets.Count, context.LightDiets.AsNoTracking().Count());
            Assert.AreEqual(boxContents.Count, context.BoxContents.AsNoTracking().Count());
            Assert.AreEqual(portionSizes.Count, context.PortionSizes.AsNoTracking().Count());
            Assert.AreEqual(lightDiets.Count, dbCustomer.CustomersLightDiets.Count);
            Assert.AreEqual(boxContents.Count, dbCustomer.CustomerMenuPlans.Count);
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
