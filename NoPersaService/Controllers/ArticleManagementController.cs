﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoPersaService.Database;
using NoPersaService.DTOs.Article.Answer;
using NoPersaService.DTOs.Article.RA;
using NoPersaService.Models;
using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<ArticleManagementController> logger;
        private readonly IMapper mapper;

        public ArticleManagementController(ILogger<ArticleManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
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
                return Ok(mapper.Map<List<DTOArticle>>(context.Articles.AsNoTracking().Include(c => c.Customers).ToList()));
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
        public IActionResult UpdateArticles([FromBody] List<DTOArticle> dTOArticle)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                if (dTOArticle.Count == 0)
                {
                    return BadRequest("At least one article is needed.");
                }
                if (!dTOArticle.Any(x => x.IsDefault))
                {
                    return BadRequest("At least one article needs to be default.");
                }

                List<Article> articles = mapper.Map<List<Article>>(dTOArticle);

                var existingArticles = context.Articles.ToDictionary(a => a.Id);
                foreach (var article in articles)
                {
                    if (existingArticles.TryGetValue(article.Id, out var foundArticle))
                    {
                        foundArticle.Position = article.Position;
                        foundArticle.NewName = article.NewName;
                        foundArticle.NewPrice = article.NewPrice;
                        foundArticle.IsDefault = article.IsDefault;
                    }
                    else
                    {
                        context.Articles.Add(article);
                    }
                }
                context.SaveChanges();

                var articleIds = new HashSet<long>(articles.Select(a => a.Id));
                List<Article> toRemove = [.. context.Articles.Include(a => a.Customers).Where(a => !articleIds.Contains(a.Id))];
                var toRemoveIds = new HashSet<long>(toRemove.Select(a => a.Id));

                Article dbDefaultArticle = context.Articles.First(a => a.IsDefault && !toRemoveIds.Contains(a.Id));
                foreach (var article in toRemove) 
                {
                    foreach (var customer in article.Customers)
                    {
                        customer.ArticleId = dbDefaultArticle.Id;
                        customer.Article = dbDefaultArticle;
                    }
                }
                context.Articles.RemoveRange(toRemove);

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

        [HttpGet("GetArticlesForCustomer", Name = "GetArticlesForCustomer")]
        public IActionResult GetArticlesForCustomer()
        {
            try
            {
                return Ok(mapper.Map<List<DTOSelectArticleWithPrice>>(context.Articles.AsNoTracking().OrderBy(x => x.Position)));
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
    }
}
