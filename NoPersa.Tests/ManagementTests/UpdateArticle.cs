using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NoPersa.Tests.DatabaseMemory;
using NoPersa.Tests.Misc;
using NoPersaService.Controllers;
using NoPersaService.Database;
using NoPersaService.DTOs.Article.RA;
using NoPersaService.Models;
using NoPersaService.Util;

namespace NoPersa.Tests.ManagementTests
{
    [TestClass]
    public class UpdateArticle : ITest
    {
        private NoPersaDbContext context;
        private ArticleManagementController controller;
        private IMapper mapper;
        private ILogger<ArticleManagementController> logger;

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
            logger = new Mock<ILogger<ArticleManagementController>>().Object;
            controller = new ArticleManagementController(logger, context, mapper);

            SeedTestData();
        }

        public void SeedTestData()
        {
            context.Articles.AddRange(StaticArticles.GetArticles());
            context.SaveChanges();
        }

        [TestMethod]
        [TestOrder(1)]
        public void UpdateArticles()
        {
            List<Article> articles = [.. context.Articles];

            Article article = articles.FirstOrDefault(c => c.Id == 1)!;
            article.Name = "abcdefg";

            controller.UpdateArticles(mapper.Map<List<DTOArticle>>(articles));
            Article dbArticle= context.Articles.AsNoTracking().FirstOrDefault(c => c.Id == 1)!;

            Assert.AreEqual(article.Name, dbArticle.Name);
        }

        [TestMethod]
        [TestOrder(1)]
        public void InsertArticle()
        {
            List<Article> articles = [.. context.Articles];
            articles.Add(new() { Name = "abcd", Position = 9, Price = 1, NewName = "ef", NewPrice = 2, IsDefault = false });

            controller.UpdateArticles(mapper.Map<List<DTOArticle>>(articles));
            List<Article> dbArticles = [..context.Articles.AsNoTracking()];

            Assert.AreEqual(articles.Count, dbArticles.Count);
        }

        [TestMethod]
        [TestOrder(1)]
        public void RemoveArticle()
        {
            List<Article> articles = [.. context.Articles];
            articles.Remove(articles.First(a => a.Id == 1));
            articles.First(a => a.Id == 2).IsDefault = true;

            controller.UpdateArticles(mapper.Map<List<DTOArticle>>(articles));
            List<Article> dbArticles = [.. context.Articles.AsNoTracking()];

            Assert.AreEqual(articles.Count, dbArticles.Count);
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
