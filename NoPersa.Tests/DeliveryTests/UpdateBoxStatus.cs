using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Controllers;
using NoPersaService.Database;
using SharedLibrary.DTOs.Delivery;
using SharedLibrary.MappingProfiles;
using SharedLibrary.Models;
using SharedLibrary.Util;

namespace NoPersa.Tests.DeliveryTests
{
    [TestClass]
    public class UpdateBoxStatus : ITest
    {
        private NoPersaDbContext context;
        private BoxManagementController controller;
        private IMapper mapper;
        private ILogger<BoxManagementController> logger;
        private HttpClient httpClient;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NoPersaDbContext>()
            .UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging()
            .Options;

            context = new NoPersaDbContext(options, SharedLibrary.Util.ProgramBuilder.BuildServiceProvider());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ManagementProfile>();
                cfg.AddProfile<DeliveryProfile>();
                cfg.AddProfile<GastronomyProfile>();
            });

            mapper = config.CreateMapper();
            logger = new Mock<ILogger<BoxManagementController>>().Object;
            httpClient = new HttpClient();
            controller = new BoxManagementController(logger, context, mapper, httpClient);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.BoxStatuses.AddRange(StaticBoxStatus.GetBoxStatuses());
            context.SaveChanges();
        }

        [TestMethod]
        [TestOrder(1)]
        public void UpdatingBoxStatus()
        {
            List<BoxStatus> boxStatuses = [.. context.BoxStatuses.AsNoTracking()];
            BoxStatus boxStatus = boxStatuses.First(b => b.Id == 1);
            boxStatus.DeliveredBoxes = 9;

            controller.UpdateBoxStatus(mapper.Map<List<DTOBoxStatus>>(boxStatuses));

            Assert.AreEqual(boxStatus.DeliveredBoxes, context.BoxStatuses.AsNoTracking().First(b => b.Id == 1).DeliveredBoxes);
        }

        [TestMethod]
        [TestOrder(2)]
        public void UpdatingCustomersBoxStatus()
        {
            List<BoxStatus> boxStatuses = [.. context.BoxStatuses.AsNoTracking()];
            BoxStatus boxStatus = boxStatuses.First(b => b.Id == 2);
            boxStatus.ReceivedBoxes = 1;

            List<DTOCustomersBoxStatus> customersBoxStatuses = [];
            customersBoxStatuses.Add(new()
            {
                Id = boxStatus.Id,
                DeliveredBoxes = boxStatus.DeliveredBoxes,
                ReceivedBoxes = boxStatus.ReceivedBoxes
            });

            controller.UpdateUpdateCustomersBoxStatusRoutes(customersBoxStatuses);

            Assert.AreEqual(3, context.BoxStatuses.AsNoTracking().First(b => b.Id == 2).NumberOfBoxesCurrentDay);
        }

        public void Cleanup()
        {
            context.Database.CloseConnection();
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
