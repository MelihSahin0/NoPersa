using AutoMapper;
using EFCore.BulkExtensions;
using GastronomyService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace GastronomyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GastronomyManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<GastronomyManagementController> logger;
        private readonly IMapper mapper;

        public GastronomyManagementController(ILogger<GastronomyManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("GetLightDiets", Name = "GetLightDiets")]
        public IActionResult GetLightDiets()
        {
            try
            {
                List<DTOLightDiet> dTOLightDiets = mapper.Map<List<DTOLightDiet>>(context.LightDiets.AsNoTracking());

                int i = 0;
                foreach (DTOLightDiet dTOLightDiet in dTOLightDiets.OrderBy(d => d.Value))
                {
                    dTOLightDiet.Position = i;
                    i++;
                }

                return Ok(dTOLightDiets);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map routes");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting routes overview failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateLightDiets", Name = "UpdateLightDiets")]
        public IActionResult UpdateLightDiets([FromBody] List<DTOLightDiet> lightDiets)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<LightDiet> newLightDiets = [];
                List<LightDiet> oldLightDiets = [];
                foreach (var lightDietDto in lightDiets)
                {
                    LightDiet lightDiet = mapper.Map<LightDiet>(lightDietDto);
                    lightDiet.Customers = [];

                    if (lightDiet.Id == 0)
                    {
                        newLightDiets.Add(lightDiet);
                    }
                    else
                    {
                        oldLightDiets.Add(lightDiet);
                    }
                }

                var dbExistingLightDiets = context.LightDiets.Where(r => oldLightDiets.Select(or => or.Id).Contains(r.Id)).ToList();
                foreach (var dbExistingLightDiet in dbExistingLightDiets)
                {
                    var dbUpdatedLightDiet = oldLightDiets.FirstOrDefault(or => or.Id == dbExistingLightDiet.Id);
                    if (dbUpdatedLightDiet != null)
                    {
                        dbExistingLightDiet.Name = dbUpdatedLightDiet.Name;
                    }
                }

                var dbNotFoundLightDiets = context.LightDiets.Where(er => !oldLightDiets.Select(or => or.Id).Contains(er.Id)).ToList();
                
                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundLightDiets.Count; j += batchSize)
                {
                    var batch = dbNotFoundLightDiets.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.LightDiets.AddRange(newLightDiets);

                context.SaveChanges();
                transaction.Commit();

                return Ok(lightDiets);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map light diet");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating light diet failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
