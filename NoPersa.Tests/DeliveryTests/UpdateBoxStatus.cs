using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Controllers;
using NoPersaService.Database;
using NoPersaService.DTOs.Box.RA;
using NoPersaService.DTOs.Box.Receive;
using NoPersaService.Models;
using NoPersaService.Util;

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
                Id = IdEncryption.EncryptId(boxStatus.Id),
                DeliveredBoxes = boxStatus.DeliveredBoxes,
                ReceivedBoxes = boxStatus.ReceivedBoxes
            });

            controller.UpdateCustomersBoxStatusRoutes(customersBoxStatuses);

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
