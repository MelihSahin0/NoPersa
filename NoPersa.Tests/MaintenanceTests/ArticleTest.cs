using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Database;
using NoPersaService.Services;
using SharedLibrary.Models;
using SharedLibrary.Util;

namespace NoPersa.Tests.MaintenanceTests
{
    [TestClass]
    public class ArticleTest
    {
        private NoPersaDbContext context;
        private ArticleService service;
        private IServiceProvider serviceProvider;
        private ILogger<ArticleService> logger;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NoPersaDbContext>()
            .UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging()
            .Options;

            context = new NoPersaDbContext(options, SharedLibrary.Util.ProgramBuilder.BuildServiceProvider());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            logger = new Mock<ILogger<ArticleService>>().Object;

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(x => x.GetService(typeof(NoPersaDbContext)))
                               .Returns(context);

            serviceProvider = serviceProviderMock.Object;

            service = new ArticleService(serviceProvider, logger);

            SeedTestData();
        }


        public void SeedTestData()
        {
            context.Articles.AddRange(StaticArticles.GetArticles());
            context.SaveChanges();
        }


        [TestMethod]
        [TestOrder(1)]
        public async Task SetArticleAsync()
        {
            Article article = context.Articles.AsNoTracking().First(a => a.Id == 1);
            Assert.AreNotEqual(article.Name, article.NewName);

            Maintenance maintenance = new() { Id = 99, Type = MaintenanceTypes.Article.ToString(), Date = DateTime.Today.AddDays(-1) };
            context.Maintenances.Add(maintenance);
            context.SaveChanges();
            await service.CatchUp(context, maintenance);

            Article dbArticle = context.Articles.AsNoTracking().First(a => a.Id == 1);
            Assert.AreEqual(dbArticle.Name, dbArticle.NewName);
            Assert.AreEqual(dbArticle.Price, dbArticle.NewPrice);
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
