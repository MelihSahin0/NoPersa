﻿using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Controllers;
using NoPersaService.Database;
using NoPersaService.DTOs.Gastro.RA;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersa.Tests.GastronomyTests
{
    [TestClass]
    public class UpdateBoxConfig : ITest
    {
        private NoPersaDbContext context;
        private GastronomyManagementController controller;
        private IMapper mapper;
        private ILogger<GastronomyManagementController> logger;

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

            mapper = serviceProvider.GetRequiredService<IMapper>();
            logger = new Mock<ILogger<GastronomyManagementController>>().Object;
            controller = new GastronomyManagementController(logger, context, mapper);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.Customers.AddRange(StaticCustomers.GetCustomers());
            context.BoxContents.AddRange(StaticBoxContents.GetBoxContents());
            context.PortionSizes.AddRange(StaticPortionSizes.GetPortionSizes());
            context.SaveChanges();

            List<CustomersMenuPlan> customersMenuPlans = [];
            customersMenuPlans.AddRange(StaticCustomersMenuPlan.GetCustomersMenuPlan());
            context.BulkInsert(customersMenuPlans);
        }

        [TestMethod]
        [TestOrder(1)]
        public void RenamePortionSize()
        {
            List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];
            portionSizes.FirstOrDefault(r => r.Id == 1)!.Name = "huge";

            List<DTOPortionSize> dTOPortionSizes = mapper.Map<List<DTOPortionSize>>(portionSizes);

            controller.UpdatePortionSizes(dTOPortionSizes);

            Assert.AreEqual("huge", context.PortionSizes.FirstOrDefault(r => r.Id == 1)!.Name);
        }

        [TestMethod]
        [TestOrder(2)]
        public void AddPortionSize()
        {
            List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];
            portionSizes.Add(new() { Id = 0, Name = "tiny", Position = 2, IsDefault = false });

            List<DTOPortionSize> dTOPortionSizes = mapper.Map<List<DTOPortionSize>>(portionSizes);

            controller.UpdatePortionSizes(dTOPortionSizes);

            Assert.AreEqual(portionSizes.Count, context.PortionSizes.Count());
        }

        [TestMethod]
        [TestOrder(3)]
        public void DeletePortionSize()
        {
            List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];
            portionSizes.RemoveAll(r => r.Name == "tiny");

            List<DTOPortionSize> dTOPortionSizes = mapper.Map<List<DTOPortionSize>>(portionSizes);

            controller.UpdatePortionSizes(dTOPortionSizes);

            Assert.AreEqual(portionSizes.Count, context.PortionSizes.Count());
        }

        [TestMethod]
        [TestOrder(4)]
        public void RenameBoxContent()
        {
            List<BoxContent> boxContents = [.. context.BoxContents.AsNoTracking()];
            boxContents.FirstOrDefault(r => r.Id == 1)!.Name = "Salad";

            List<DTOBoxContent> dTOBoxContents = mapper.Map<List<DTOBoxContent>>(boxContents);

            controller.UpdateBoxContents(dTOBoxContents);

            Assert.AreEqual("Salad", context.BoxContents.FirstOrDefault(r => r.Id == 1)!.Name);
        }

        [TestMethod]
        [TestOrder(5)]
        public void AddBoxContent()
        {
            List<BoxContent> boxContents = [.. context.BoxContents.AsNoTracking()];
            boxContents.Add(new() { Id = 0, Name = "Surprise", Position = 99 });

            List<DTOBoxContent> dTOBoxContents = mapper.Map<List<DTOBoxContent>>(boxContents);

            controller.UpdateBoxContents(dTOBoxContents);

            Assert.AreEqual(boxContents.Count, context.BoxContents.Count());
            Assert.AreEqual(boxContents.Count, context.CustomersMenuPlans.Where(cmp => cmp.CustomerId == 1).Count());
        }

        [TestMethod]
        [TestOrder(6)]
        public void DeleteBoxContent()
        {
            List<BoxContent> boxContents = [.. context.BoxContents.AsNoTracking()];
            boxContents.RemoveAll(r => r.Name == "Surprise");

            List<DTOBoxContent> dTOBoxContents = mapper.Map<List<DTOBoxContent>>(boxContents);

            controller.UpdateBoxContents(dTOBoxContents);

            Assert.AreEqual(boxContents.Count, context.BoxContents.Count());
            Assert.AreEqual(boxContents.Count, context.CustomersMenuPlans.Where(cmp => cmp.CustomerId == 1).Count());
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
