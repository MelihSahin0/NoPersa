using AutoMapper;
using EFCore.BulkExtensions;
using GastronomyService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SharedLibrary.DTOs.Gastro;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Models;
using SharedLibrary.Util;
using System.ComponentModel.DataAnnotations;
using Holiday = SharedLibrary.Models.Holiday;
using Route = SharedLibrary.Models.Route;

namespace GastronomyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GastronomyManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<GastronomyManagementController> logger;
        private readonly IMapper mapper;
        private readonly string currentCountry = "at"; //TODO: Will be removed in the future

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
                bool updateCustomersLightDiets = dTOLightDiets.Any(l => l.Id == 0);

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

                var oldDietIds = oldLightDiets.Select(d => d.Id).ToHashSet();
                var dbExistingLightDiets = context.LightDiets.Where(d => oldDietIds.Contains(d.Id)).ToList();
                foreach (var dbExistingLightDiet in dbExistingLightDiets)
                {
                    var dbUpdatedLightDiet = oldLightDiets.FirstOrDefault(or => or.Id == dbExistingLightDiet.Id);
                    if (dbUpdatedLightDiet != null)
                    {
                        dbExistingLightDiet.Name = dbUpdatedLightDiet.Name;
                    }
                }

                var dbNotFoundLightDiets = context.LightDiets.Where(er => !oldDietIds.Contains(er.Id)).ToList();
                
                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundLightDiets.Count; j += batchSize)
                {
                    var batch = dbNotFoundLightDiets.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.LightDiets.AddRange(newLightDiets);
                context.SaveChanges();

                if (updateCustomersLightDiets)
                {
                    List<LightDiet> lightDiets = [.. context.LightDiets]; 

                    foreach (Customer customer in context.Customers.Include(cld => cld.CustomersLightDiets))
                    {
                        foreach (LightDiet lightDiet in lightDiets)
                        {
                            CustomersLightDiet? dbCustomersLightDiet = customer.CustomersLightDiets.FirstOrDefault(cld => cld.LightDietId == lightDiet.Id);

                            if (dbCustomersLightDiet == null)
                            {
                                dbCustomersLightDiet = new()
                                {
                                    Customer = customer,
                                    CustomerId = customer.Id,
                                    LightDietId = lightDiet.Id,
                                    LightDiet = lightDiet,
                                    Selected = false
                                };
                                customer.CustomersLightDiets.Add(dbCustomersLightDiet);
                            }
                        }
                    }

                    context.BulkSaveChanges();
                }

                transaction.Commit();

                return Ok();
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
                bool updateCustomersMenu = dTOBoxContents.Any(l => l.Id == 0) && context.PortionSizes.AsNoTracking().Any();

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

                var oldBoxId = oldBoxContents.Select(d => d.Id).ToHashSet();
                var dbExistingLightDiets = context.BoxContents.Where(d => oldBoxId.Contains(d.Id)).ToList();
                foreach (var dbExistingLightDiet in dbExistingLightDiets)
                {
                    var dbUpdatedLightDiet = oldBoxContents.FirstOrDefault(or => or.Id == dbExistingLightDiet.Id);
                    if (dbUpdatedLightDiet != null)
                    {
                        dbExistingLightDiet.Name = dbUpdatedLightDiet.Name;
                    }
                }

                var dbNotFoundLightDiets = context.BoxContents.Where(er => !oldBoxId.Contains(er.Id)).ToList();

                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundLightDiets.Count; j += batchSize)
                {
                    var batch = dbNotFoundLightDiets.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.BoxContents.AddRange(newBoxContents);
                context.SaveChanges();

                if (updateCustomersMenu)
                {
                    List<BoxContent> boxContents = [.. context.BoxContents];
                    PortionSize portionSize = context.PortionSizes.First(p => p.Position == 0);

                    foreach (Customer customer in context.Customers.Include(cmp => cmp.CustomerMenuPlans))
                    {
                        foreach (BoxContent boxContent in boxContents)
                        {
                            CustomersMenuPlan? dBCustomersMenuPlan = customer.CustomerMenuPlans.FirstOrDefault(cmp => cmp.BoxContentId == boxContent.Id);

                            if (dBCustomersMenuPlan == null)
                            {
                                dBCustomersMenuPlan = new()
                                {
                                    Customer = customer,
                                    CustomerId = customer.Id,
                                    BoxContentId = boxContent.Id,
                                    BoxContent = boxContent,
                                    PortionSize = portionSize,
                                    PortionSizeId = portionSize.Id
                                };
                                customer.CustomerMenuPlans.Add(dBCustomersMenuPlan);
                            }
                        }
                    }

                    context.BulkSaveChanges();
                }

                transaction.Commit();

                return Ok();
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
                bool updateCustomersMenu = dTOPortionSizes.Count != 0 && dTOPortionSizes.FirstOrDefault(p => p.Position == 0)?.Id == 0;

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

                var oldPortionId = oldPortionSizes.Select(p => p.Id).ToHashSet();
                var dbExistingPortionSizes = context.PortionSizes.Where(p => oldPortionId.Contains(p.Id)).ToList();
                foreach (var dbExistingPortionSize in dbExistingPortionSizes)
                {
                    var dbUpdatedPortionSize = oldPortionSizes.FirstOrDefault(or => or.Id == dbExistingPortionSize.Id);
                    if (dbUpdatedPortionSize != null)
                    {
                        dbExistingPortionSize.Name = dbUpdatedPortionSize.Name;
                        dbExistingPortionSize.Position = dbUpdatedPortionSize.Position;
                    }
                }

                var dbNotFoundPortionSizes = context.PortionSizes.Where(er => !oldPortionId.Contains(er.Id)).ToList();
                if (dbNotFoundPortionSizes.Count > 0)
                {
                    updateCustomersMenu = true;
                }

                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundPortionSizes.Count; j += batchSize)
                {
                    var batch = dbNotFoundPortionSizes.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.PortionSizes.AddRange(newPortionSizes);
                context.SaveChanges();

                if (updateCustomersMenu)
                {
                    List<BoxContent> boxContents = [.. context.BoxContents];
                    PortionSize portionSize = context.PortionSizes.First(p => p.Position == 0);

                    foreach (Customer customer in context.Customers.Include(cmp => cmp.CustomerMenuPlans))
                    {
                        foreach (BoxContent boxContent in boxContents)
                        {
                            CustomersMenuPlan? dBCustomersMenuPlan = customer.CustomerMenuPlans.FirstOrDefault(cmp => cmp.BoxContentId == boxContent.Id);

                            if (dBCustomersMenuPlan == null)
                            {
                                dBCustomersMenuPlan = new()
                                {
                                    Customer = customer,
                                    CustomerId = customer.Id,
                                    BoxContentId = boxContent.Id,
                                    BoxContent = boxContent,
                                    PortionSize = portionSize,
                                    PortionSizeId = portionSize.Id
                                };
                                customer.CustomerMenuPlans.Add(dBCustomersMenuPlan);
                            }
                        }
                    }

                    context.BulkSaveChanges();
                }

                transaction.Commit();

                return Ok();
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

        [HttpPost("GetFoodOverview", Name = "GetFoodOverview")]
        public async Task<IActionResult> GetFoodOverview([FromBody] DTOSelectedDay dTOSelectedDay)
        {
            try
            {
                List<Route> dbRoutes = await context.Routes.AsNoTracking().Where(r => r.Id != int.MinValue).Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                       .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                       .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month))
                                                                 .ThenInclude(d => d.DailyOverviews.Where(x => x.DayOfMonth == dTOSelectedDay.Day))
                                       .Include(r => r.Customers).ThenInclude(cmp => cmp.CustomerMenuPlans)
                                       .Include(r => r.Customers).ThenInclude(cld => cld.CustomersLightDiets)
                                       .ToListAsync();
                Holiday? holiday = await context.Holidays.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == dTOSelectedDay.Year && h.Month == dTOSelectedDay.Month && h.Day == dTOSelectedDay.Day);

                List<DTORoutesFoodSummary> dTORoutesFoodSummaryList = [];
                List<DTOLightDietSummary> dTOLightDietSummaryList = mapper.Map<List<DTOLightDietSummary>>(context.LightDiets.AsNoTracking());
                List<DTOPortionSizeSummary> dTOPortionSizeSummaryList = mapper.Map<List<DTOPortionSizeSummary>>(context.PortionSizes.AsNoTracking());
                List<DTOBoxContentSummary> dTOBoxContentSummaryList = mapper.Map<List<DTOBoxContentSummary>>(context.BoxContents.AsNoTracking());

                foreach (DTOBoxContentSummary dTOBoxContentSummary in dTOBoxContentSummaryList)
                {
                    dTOBoxContentSummary.PortionSizeSummary = dTOPortionSizeSummaryList.Select(x => x.Clone()).ToList();
                }

                DTORoutesFoodSummary dTOSummaryRoutesFoodSummary = new()
                {
                    RouteName = "Summary",
                    LightDietSummary = dTOLightDietSummaryList.Select(x => x.Clone()).ToList(),
                    BoxContentSummary = dTOBoxContentSummaryList.Select(x => x.Clone()).ToList()
                };

                foreach (Route dbRoute in dbRoutes.OrderBy(r => r.Position))
                {
                    DTORoutesFoodSummary dTORoutesFoodSummary = new()
                    {
                        RouteName = dbRoute.Name,
                        LightDietSummary = dTOLightDietSummaryList.Select(x => x.Clone()).ToList(),
                        BoxContentSummary = dTOBoxContentSummaryList.Select(x => x.Clone()).ToList()
                    };

                    foreach (Customer dbCustomer in dbRoute.Customers ?? [])
                    {
                        bool toDeliver = false;

                        MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month);
                      
                        if (dbFoundOverview != null)
                        {
                            int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
                            toDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day));
                        }
                        else //Safety Measure
                        {
                            toDeliver = CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day);
                        }

                        if (toDeliver)
                        {
                            foreach (CustomersLightDiet lightDiet in dbCustomer.CustomersLightDiets)
                            {
                                if (lightDiet.Selected)
                                {
                                    dTORoutesFoodSummary.LightDietSummary.First(l => l.Id == lightDiet.LightDietId).Value += 1;
                                    dTOSummaryRoutesFoodSummary.LightDietSummary.First(l => l.Id == lightDiet.LightDietId).Value += 1;

                                }
                            }

                            foreach (CustomersMenuPlan menuPlan in dbCustomer.CustomerMenuPlans)
                            {
                                dTORoutesFoodSummary.BoxContentSummary.First(b => b.Id == menuPlan.BoxContentId).PortionSizeSummary!.First(p => p.Id == menuPlan.PortionSizeId).Value += 1;
                                dTOSummaryRoutesFoodSummary.BoxContentSummary.First(b => b.Id == menuPlan.BoxContentId).PortionSizeSummary!.First(p => p.Id == menuPlan.PortionSizeId).Value += 1;
                            }
                        }
                    }
                    dTORoutesFoodSummaryList.Add(dTORoutesFoodSummary);
                }

                dTORoutesFoodSummaryList.Insert(0, dTOSummaryRoutesFoodSummary);

                return Ok(dTORoutesFoodSummaryList);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map food overview");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting map food overview failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetRoutesFoodOverview", Name = "GetRoutesFoodOverview")]
        public async Task<IActionResult> GetRoutesFoodOverview([FromBody] DTOSelectedDay dTOSelectedDay)
        {
            try
            {
                List<Route> dbRoutes = await context.Routes.AsNoTracking().Where(r => r.Id != int.MinValue)
                                       .Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                       .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                       .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month))
                                                                 .ThenInclude(d => d.DailyOverviews.Where(x => x.DayOfMonth == dTOSelectedDay.Day))
                                       .Include(r => r.Customers).ThenInclude(cmp => cmp.CustomerMenuPlans).ThenInclude(bc => bc.BoxContent)
                                       .Include(r => r.Customers).ThenInclude(cmp => cmp.CustomerMenuPlans).ThenInclude(pS => pS.PortionSize)
                                       .Include(r => r.Customers).ThenInclude(cld => cld.CustomersLightDiets).ThenInclude(ld => ld.LightDiet)
                                       .ToListAsync();
                Holiday? holiday = await context.Holidays.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == dTOSelectedDay.Year && h.Month == dTOSelectedDay.Month && h.Day == dTOSelectedDay.Day);

                List<DTORoutesFoodOverview> dTORoutesFoodOverviews = [];

                foreach (Route dbRoute in dbRoutes.OrderBy(r => r.Position))
                {
                    List<DTOCustomersFood> dTOCustomersFoods = [];

                    foreach (Customer dbCustomer in dbRoute.Customers ?? [])
                    {
                        bool toDeliver = false;

                        MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month);

                        if (dbFoundOverview != null)
                        {
                            int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
                            toDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day));
                        }
                        else //Safety Measure
                        {
                            toDeliver = CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day);
                        }

                        if (toDeliver)
                        {
                            List<string> lightDiets = [];
                            List<DTOCustomersBoxContent> dTOCustomersBoxContents = [];

                            foreach (CustomersLightDiet lightDiet in dbCustomer.CustomersLightDiets)
                            {
                                if (lightDiet.Selected)
                                {
                                    lightDiets.Add(lightDiet.LightDiet.Name);
                                }
                            }

                            foreach (CustomersMenuPlan menuPlan in dbCustomer.CustomerMenuPlans)
                            {
                                dTOCustomersBoxContents.Add(new()
                                {
                                    BoxName = menuPlan.BoxContent.Name,
                                    PortionSize = menuPlan.PortionSize.Name
                                });
                            }

                            dTOCustomersFoods.Add(new()
                            {
                                Name = dbCustomer.Name,
                                Position = dbCustomer.Position,
                                LightDiets = lightDiets,
                                BoxContents = dTOCustomersBoxContents
                            });
                        }
                    }

                    dTORoutesFoodOverviews.Add(new()
                    {
                        Name = dbRoute.Name,
                        Position = dbRoute.Position,
                        CustomersFoods = dTOCustomersFoods
                    });
                }

                return Ok(dTORoutesFoodOverviews);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map food overview");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting map food overview failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
