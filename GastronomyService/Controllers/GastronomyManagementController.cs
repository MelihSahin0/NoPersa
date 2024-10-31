using AutoMapper;
using EFCore.BulkExtensions;
using GastronomyService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
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
                logger.LogError(e, "Could not map light diets");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting light diets failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateLightDiets", Name = "UpdateLightDiets")]
        public IActionResult UpdateLightDiets([FromBody] List<DTOLightDiet> dTOLightDiets)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<LightDiet> newLightDiets = [];
                List<LightDiet> oldLightDiets = [];
                foreach (var dTOLightDiet in dTOLightDiets)
                {
                    LightDiet lightDiet = mapper.Map<LightDiet>(dTOLightDiet);

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

                return Ok(dTOLightDiets);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map light diets");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating light diets failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpGet("GetBoxContents", Name = "GetBoxContents")]
        public IActionResult GetBoxContents()
        {
            try
            {
                List<DTOBoxContent> dTOBoxContents= mapper.Map<List<DTOBoxContent>>(context.BoxContents.AsNoTracking());

                int i = 0;
                foreach (DTOBoxContent dTOBoxContent in dTOBoxContents.OrderBy(d => d.Value))
                {
                    dTOBoxContent.Position = i;
                    i++;
                }

                return Ok(dTOBoxContents);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map box contents");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting box contents failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateBoxContents", Name = "UpdateBoxContents")]
        public IActionResult UpdateBoxContents([FromBody] List<DTOBoxContent> dTOBoxContents)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<BoxContent> newBoxContents = [];
                List<BoxContent> oldBoxContents = [];
                foreach (var dTOBoxContent in dTOBoxContents)
                {
                    BoxContent boxContent = mapper.Map<BoxContent>(dTOBoxContent);

                    if (boxContent.Id == 0)
                    {
                        newBoxContents.Add(boxContent);
                    }
                    else
                    {
                        oldBoxContents.Add(boxContent);
                    }
                }

                var dbExistingLightDiets = context.BoxContents.Where(r => oldBoxContents.Select(or => or.Id).Contains(r.Id)).ToList();
                foreach (var dbExistingLightDiet in dbExistingLightDiets)
                {
                    var dbUpdatedLightDiet = oldBoxContents.FirstOrDefault(or => or.Id == dbExistingLightDiet.Id);
                    if (dbUpdatedLightDiet != null)
                    {
                        dbExistingLightDiet.Name = dbUpdatedLightDiet.Name;
                    }
                }

                var dbNotFoundLightDiets = context.BoxContents.Where(er => !oldBoxContents.Select(or => or.Id).Contains(er.Id)).ToList();

                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundLightDiets.Count; j += batchSize)
                {
                    var batch = dbNotFoundLightDiets.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.BoxContents.AddRange(newBoxContents);

                context.SaveChanges();
                transaction.Commit();

                return Ok(dTOBoxContents);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map box contents");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating box contents failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpGet("GetPortionSizes", Name = "GetPortionSizes")]
        public IActionResult GetPortionSizes()
        {
            try
            {
                List<DTOPortionSize> dTOPortionSizes = mapper.Map<List<DTOPortionSize>>(context.PortionSizes.AsNoTracking());

                int i = 0;
                foreach (DTOPortionSize dTOPortionSize in dTOPortionSizes.OrderBy(d => d.Value))
                {
                    dTOPortionSize.Position = i;
                    i++;
                }

                return Ok(dTOPortionSizes);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map portion sizes");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting portion sizes failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdatePortionSizes", Name = "UpdatePortionSizes")]
        public IActionResult UpdatePortionSizes([FromBody] List<DTOPortionSize> dTOPortionSizes)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<PortionSize> newPortionSizes = [];
                List<PortionSize> oldPortionSizes = [];
                foreach (var dTOPortionSize in dTOPortionSizes)
                {
                    PortionSize portionSize = mapper.Map<PortionSize>(dTOPortionSize);

                    if (portionSize.Id == 0)
                    {
                        newPortionSizes.Add(portionSize);
                    }
                    else
                    {
                        oldPortionSizes.Add(portionSize);
                    }
                }

                var dbExistingPortionSizes = context.PortionSizes.Where(r => oldPortionSizes.Select(or => or.Id).Contains(r.Id)).ToList();
                foreach (var dbExistingPortionSize in dbExistingPortionSizes)
                {
                    var dbUpdatedPortionSize = oldPortionSizes.FirstOrDefault(or => or.Id == dbExistingPortionSize.Id);
                    if (dbUpdatedPortionSize != null)
                    {
                        dbExistingPortionSize.Name = dbUpdatedPortionSize.Name;
                    }
                }

                var dbNotFoundPortionSizes = context.PortionSizes.Where(er => !oldPortionSizes.Select(or => or.Id).Contains(er.Id)).ToList();

                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundPortionSizes.Count; j += batchSize)
                {
                    var batch = dbNotFoundPortionSizes.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.PortionSizes.AddRange(newPortionSizes);

                context.SaveChanges();
                transaction.Commit();

                return Ok(dTOPortionSizes);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map portion sizes");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating portion sizes failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
