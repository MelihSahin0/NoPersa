using AutoMapper;
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
    public class UpdateFoodWish : ITest
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
            context.FoodWishes.AddRange(StaticFoodWishes.GetFoodWishes());        
            context.SaveChanges();

        }

        [TestMethod]
        [TestOrder(1)]
        public void RenameFoodWish()
        {
            List<FoodWish> foodWishes = [.. context.FoodWishes.AsNoTracking()];
            foodWishes.FirstOrDefault(r => r.Id == 1)!.Name = "Reis";

            List<DTOFoodWish> dTOFoodWishes = mapper.Map<List<DTOFoodWish>>(foodWishes);
         
            controller.UpdateFoodWishes(dTOFoodWishes);

            Assert.AreEqual("Reis", context.FoodWishes.FirstOrDefault(r => r.Id == 1)!.Name);
        }

        [TestMethod]
        [TestOrder(2)]
        public void AddFoodWish()
        {
            List<FoodWish> foodWishes = [.. context.FoodWishes.AsNoTracking()];
            foodWishes.Add(new() { Id = 0, Name = "Milch", IsIngredient = true, Position = 99 });

            List<DTOFoodWish> dTOFoodWishes = mapper.Map<List<DTOFoodWish>>(foodWishes);

            controller.UpdateFoodWishes(dTOFoodWishes);

            Assert.AreEqual(foodWishes.Count, context.FoodWishes.Count());
        }

        [TestMethod]
        [TestOrder(3)]
        public void DeleteFoodWish()
        {
            List<FoodWish> foodWishes = [.. context.FoodWishes.AsNoTracking()];
            foodWishes.RemoveAll(r => r.Name == "Sugar");

            List<DTOFoodWish> dTOFoodWishes = mapper.Map<List<DTOFoodWish>>(foodWishes);

            controller.UpdateFoodWishes(dTOFoodWishes);

            Assert.AreEqual(foodWishes.Count, context.FoodWishes.Count());
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
