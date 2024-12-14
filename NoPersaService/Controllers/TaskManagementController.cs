using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoPersaService.Database;
using SharedLibrary.DTOs.AnswerDTO;
using SharedLibrary.Models;
using SharedLibrary.Util;
using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class TaskManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<TaskManagementController> logger;
        private readonly IMapper mapper;

        public TaskManagementController(ILogger<TaskManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("GetArticleTask", Name = "GetArticleTask")]
        public IActionResult GetArticleTask()
        {      
            try
            {
                DTOSelectedNullDay dTOSelectedDay = new();

                Maintenance? maintenance = context.Maintenances.FirstOrDefault(m => m.Type == MaintenanceTypes.Article.ToString());
                if (maintenance != null)
                {
                    dTOSelectedDay.Year = maintenance.Date.Year;
                    dTOSelectedDay.Month = maintenance.Date.Month;
                    dTOSelectedDay.Day = maintenance.Date.Day;
                }

                return Ok(dTOSelectedDay);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map task");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed getting task");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateArticleTask", Name = "UpdateArticleTask")]
        public IActionResult UpdateArticleTask([FromBody] DTOSelectedNullDay dTOSelectedDay)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                if (dTOSelectedDay.Year == null || dTOSelectedDay.Month == null || dTOSelectedDay.Day == null)
                {
                    Maintenance? maintenance = context.Maintenances.FirstOrDefault(m => m.Type == MaintenanceTypes.Article.ToString());

                    if (maintenance != null)
                    {
                        context.Remove(maintenance);

                        context.SaveChanges();
                        transaction.Commit();
                    }
                }
                else
                {
                    DateTime dateTime = new((int)dTOSelectedDay.Year, (int)dTOSelectedDay.Month, (int)dTOSelectedDay.Day);

                    if (dateTime <= DateTime.Today)
                    {
                        return BadRequest("You can set this task to run at the next day");
                    }

                    Maintenance? maintenance = context.Maintenances.FirstOrDefault(m => m.Type == MaintenanceTypes.Article.ToString());
                    if (maintenance == null)
                    {
                        context.Maintenances.Add(new Maintenance() { Type = MaintenanceTypes.Article.ToString(), Date = dateTime });
                    }
                    else
                    {
                        maintenance.Date = dateTime;
                    }

                    context.SaveChanges();
                    transaction.Commit();
                }

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map task");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Failed updating task");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
