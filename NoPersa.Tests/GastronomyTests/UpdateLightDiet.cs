﻿using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Controllers;
using NoPersaService.Database;
using SharedLibrary.DTOs.Gastro;
using SharedLibrary.MappingProfiles;
using SharedLibrary.Models;

namespace NoPersa.Tests.GastronomyTests
{
    [TestClass]
    public class UpdateLightDiet : ITest
    {
        private NoPersaDbContext context;
        private GastronomyManagementController controller;
        private IMapper mapper;
        private ILogger<GastronomyManagementController> logger;

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
            logger = new Mock<ILogger<GastronomyManagementController>>().Object;
            controller = new GastronomyManagementController(logger, context, mapper);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.Customers.AddRange(StaticCustomers.GetCustomers());
            context.LightDiets.AddRange(StaticLightDiets.GetLightDiets());        
            context.SaveChanges();

            List<CustomersLightDiet> customersLightDiets = [];
            customersLightDiets.AddRange(StaticCustomersLightDiets.GetCustomersLightDiets());
            context.BulkInsert(customersLightDiets);
        }

        [TestMethod]
        [TestOrder(1)]
        public void RenameLightDiet()
        {
            List<LightDiet> lightDiets = [.. context.LightDiets.AsNoTracking()];
            lightDiets.FirstOrDefault(r => r.Id == 1)!.Name = "Vegetarisch";

            List<DTOLightDiet> dTOLightDiets = mapper.Map<List<DTOLightDiet>>(lightDiets);
         
            controller.UpdateLightDiets(dTOLightDiets);

            Assert.AreEqual("Vegetarisch", context.LightDiets.FirstOrDefault(r => r.Id == 1)!.Name);
        }

        [TestMethod]
        [TestOrder(2)]
        public void AddLightDiet()
        {
            List<LightDiet> lightDiets = [.. context.LightDiets.AsNoTracking()];
            lightDiets.Add(new() { Id = 0, Name = "Schweinefleisch", Position = 99 });

            List<DTOLightDiet> dTOLightDiets = mapper.Map<List<DTOLightDiet>>(lightDiets);

            controller.UpdateLightDiets(dTOLightDiets);

            Assert.AreEqual(lightDiets.Count, context.LightDiets.Count());
        }

        [TestMethod]
        [TestOrder(3)]
        public void DeleteLightDiet()
        {
            List<LightDiet> lightDiets = [.. context.LightDiets.AsNoTracking()];
            lightDiets.RemoveAll(r => r.Name == "Schweinefleisch");

            List<DTOLightDiet> dTOLightDiets = mapper.Map<List<DTOLightDiet>>(lightDiets);

            controller.UpdateLightDiets(dTOLightDiets);

            Assert.AreEqual(lightDiets.Count, context.LightDiets.Count());
            Assert.AreEqual(lightDiets.Count, context.CustomersLightDiets.Where(c => c.CustomerId == 1).Count());
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
