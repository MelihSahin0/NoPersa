﻿using AutoMapper;
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
    public class UpdateRoute : ITest
    {
        private NoPersaDbContext context;
        private DeliveryManagementController controller;
        private IMapper mapper;
        private ILogger<DeliveryManagementController> logger;
        private HttpClient httpClient;

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
                cfg.AddProfile<ManagementProfile>();
                cfg.AddProfile<DeliveryProfile>();
                cfg.AddProfile<GastronomyProfile>();
            });

            mapper = config.CreateMapper();
            logger = new Mock<ILogger<DeliveryManagementController>>().Object;
            httpClient = new HttpClient();
            controller = new DeliveryManagementController(logger, context, mapper, httpClient);
            
            SeedTestData();
        }

        public void SeedTestData()
        {
            context.Routes.AddRange(StaticRoutes.GetRoutes());
            context.SaveChanges();
        }

        [TestMethod]
        [TestOrder(1)]
        public void RenameRoute()
        {
            List<Route> routes = [.. context.Routes.AsNoTracking()];
            routes.FirstOrDefault(r => r.Id == 1)!.Name = "Route 0";

            controller.UpdateRoutes(mapper.Map<List<DTORouteSummary>>(routes));

            Assert.AreEqual("Route 0", context.Routes.FirstOrDefault(r => r.Id == 1)!.Name);
        }

        [TestMethod]
        [TestOrder(2)]
        public void OrderRoute()
        {
            List<Route> routes = [.. context.Routes.AsNoTracking()];
            routes.FirstOrDefault(r => r.Id == 1)!.Position = 2;
            routes.FirstOrDefault(r => r.Id == 2)!.Position = 3;
            routes.FirstOrDefault(r => r.Id == 3)!.Position = 1;

            controller.UpdateRoutes(mapper.Map<List<DTORouteSummary>>(routes));

            CollectionAssert.AreEqual(new List<int> {int.MinValue, 3,1,2}, context.Routes.OrderBy(r => r.Position)!.Select(r => r.Id).ToList());
        }

        [TestMethod]
        [TestOrder(3)]
        public void AddRoute()
        {
            List<Route> routes = [.. context.Routes.AsNoTracking()];
            routes.Add(new() { Id = 0, Name = "Route 4", Position = 4 });

            controller.UpdateRoutes(mapper.Map<List<DTORouteSummary>>(routes));

            Assert.AreEqual(routes.Count, context.Routes.Count());
        }

        [TestMethod]
        [TestOrder(4)]
        public void DeleteRoute()
        {
            List<Route> routes = [.. context.Routes.AsNoTracking()];
            routes.RemoveAll(r => r.Id == 3);

            controller.UpdateRoutes(mapper.Map<List<DTORouteSummary>>(routes));

            Assert.AreEqual(routes.Count, context.Routes.Count());
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
