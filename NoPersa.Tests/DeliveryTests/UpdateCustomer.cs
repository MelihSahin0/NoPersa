using AutoMapper;
using DeliveryService.Controllers;
using DeliveryService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using SharedLibrary.DTOs.Delivery;
using SharedLibrary.MappingProfiles;
using SharedLibrary.Models;

namespace NoPersa.Tests.DeliveryTests
{
    [TestClass]
    public class UpdateCustomer : ITest
    {
        private NoPersaDbContext context;
        private DeliveryManagementController controller;
        private IMapper mapper;
        private ILogger<DeliveryManagementController> logger;

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
            });

            mapper = config.CreateMapper();
            logger = new Mock<ILogger<DeliveryManagementController>>().Object;
            controller = new DeliveryManagementController(logger, context, mapper);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.Customers.AddRange(StaticCustomers.GetCustomers());
            context.SaveChanges();
        }

        [Ignore("This test will fail, because the in memory database has problems with the batch update")]
        [TestMethod]
        [TestOrder(1)]
        public void UpdateCustomerSequenceChangeRoute()
        {
            List<Route> routes = [.. context.Routes.AsNoTracking().Include(r => r.Customers)];

            var customerToMove = routes.FirstOrDefault(r => r.Id == 2)!.Customers.FirstOrDefault(c => c.Id == 2)!;
            routes.FirstOrDefault(r => r.Id == 2)!.Customers.Remove(customerToMove);

            customerToMove.Position = 1;
            customerToMove.RouteId = 1;
            routes.FirstOrDefault(r => r.Id == 1)!.Customers.Add(customerToMove);

            var sequenceDetails = mapper.Map<List<DTOCustomersInRoute>>(routes);
            
            controller.UpdateCustomerSequence(sequenceDetails);

            Assert.AreEqual(routes.FirstOrDefault(r => r.Id == 1)!.Customers.Count, context.Routes.FirstOrDefault(r => r.Id == 1)!.Customers.Count);
            Assert.AreEqual(routes.FirstOrDefault(r => r.Id == 2)!.Customers.Count, context.Routes.FirstOrDefault(r => r.Id == 2)!.Customers.Count);
        }

        public void Cleanup()
        {
            context.Database.CloseConnection();
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
