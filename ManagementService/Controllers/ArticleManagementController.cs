using AutoMapper;
using ManagementService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.Management;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<CustomerManagementController> logger;
        private readonly IMapper mapper;

        public ArticleManagementController(ILogger<CustomerManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("GetArticles", Name = "GetArticles")]
        public IActionResult GetArticles()
        {
            try
            {
                return Ok(mapper.Map<List<DTOArticle>>(context.Articles.AsNoTracking().ToList()));
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map articles");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "failed getting articles");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateArticles", Name = "UpdateArticles")]
        public IActionResult UpdateArticlesAsync([FromBody] DTOArticle dTOArticle)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<Article> articles = mapper.Map<List<Article>>(dTOArticle);

                List<Article> toRemove = [.. context.Articles.Where(article => !articles.Any(a => a.Id == article.Id)).Include(c => c.Customers)];
                context.Articles.RemoveRange(toRemove);

                var existingArticles = context.Articles.ToDictionary(a => a.Id);
                foreach (var article in articles)
                {
                    if (existingArticles.TryGetValue(article.Id, out var foundArticle))
                    {
                        foundArticle.Position = article.Position;
                    }
                }

                context.Articles.AddRange(articles.Where(a => a.Id == 0));
                context.SaveChanges();

                Article defaultArticle = context.Articles.First(a => a.Id == 0);
                foreach (var article in toRemove) 
                {
                    foreach (var customer in article.Customers)
                    {
                        customer.ArticleId = defaultArticle.Id;
                        customer.Article = defaultArticle;
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not update articles");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "failed updating articles");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
