using AutoMapper;
using EFCore.BulkExtensions;
using GastronomyService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.Gastro;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.DTOs.Management;
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
                List<LightDiet> lightDiets = mapper.Map<List<LightDiet>>(dTOLightDiets);
                bool updateCustomersLightDiets = lightDiets.Any(l => l.Id == 0);

                var existingLightDiets = context.LightDiets.ToDictionary(a => a.Id);
                foreach (var lightDiet in lightDiets)
                {
                    if (existingLightDiets.TryGetValue(lightDiet.Id, out var foundLightDiet))
                    {
                        foundLightDiet.Name = lightDiet.Name;
                    }
                    else
                    {
                        context.LightDiets.Add(lightDiet);
                    }
                }

                var lightDietsIds = new HashSet<int>(lightDiets.Select(a => a.Id));
                List<LightDiet> toRemove = [.. context.LightDiets.Where(er => !lightDietsIds.Contains(er.Id))];
              
                const int batchSize = 1000;
                for (int j = 0; j < toRemove.Count; j += batchSize)
                {
                    var batch = toRemove.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.SaveChanges();

                if (updateCustomersLightDiets)
                {
                    List<LightDiet> dbLightDiets = [.. context.LightDiets]; 

                    foreach (Customer customer in context.Customers.Include(cld => cld.CustomersLightDiets))
                    {
                        foreach (LightDiet lightDiet in dbLightDiets)
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
                List<BoxContent> boxContents = mapper.Map<List<BoxContent>>(dTOBoxContents);
                bool updateCustomersMenu = boxContents.Any(l => l.Id == 0) && context.PortionSizes.AsNoTracking().Any();

                var existingBoxContents = context.BoxContents.ToDictionary(a => a.Id);
                foreach (var boxContent in boxContents)
                {
                    if (existingBoxContents.TryGetValue(boxContent.Id, out var foundBoxContent))
                    {
                        foundBoxContent.Name = boxContent.Name;
                    }
                    else
                    {
                        context.BoxContents.Add(boxContent);
                    }
                }

                var boxContentIds = new HashSet<int>(boxContents.Select(a => a.Id));
                List<BoxContent> toRemove = [.. context.BoxContents.Where(er => !boxContentIds.Contains(er.Id))];

                const int batchSize = 1000;
                for (int j = 0; j < toRemove.Count; j += batchSize)
                {
                    var batch = toRemove.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.SaveChanges();

                if (updateCustomersMenu)
                {
                    List<BoxContent> dbBoxContents = [.. context.BoxContents];
                    PortionSize portionSize = context.PortionSizes.First(p => p.Position == 0);

                    foreach (Customer customer in context.Customers.Include(cmp => cmp.CustomerMenuPlans))
                    {
                        foreach (BoxContent boxContent in dbBoxContents)
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
                List<PortionSize> portionSizes = mapper.Map<List<PortionSize>>(dTOPortionSizes);
                bool updateCustomersMenu = portionSizes.Count != 0 && portionSizes.FirstOrDefault(p => p.Position == 0)?.Id == 0;

                var existingPortionSizes = context.PortionSizes.ToDictionary(a => a.Id);
                foreach (var portionSize in portionSizes)
                {
                    if (existingPortionSizes.TryGetValue(portionSize.Id, out var foundPortionSize))
                    {
                        foundPortionSize.Position = portionSize.Position;
                        foundPortionSize.Name = portionSize.Name;
                    }
                    else
                    {
                        context.PortionSizes.Add(portionSize);
                    }
                }

                var portionSizeIds = new HashSet<int>(portionSizes.Select(a => a.Id));
                List<PortionSize> toRemove = [.. context.PortionSizes.Where(er => !portionSizeIds.Contains(er.Id))];

                if (toRemove.Count > 0)
                {
                    updateCustomersMenu = true;
                }

                const int batchSize = 1000;
                for (int j = 0; j < toRemove.Count; j += batchSize)
                {
                    var batch = toRemove.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.SaveChanges();

                if (updateCustomersMenu)
                {
                    List<BoxContent> dbBoxContents = [.. context.BoxContents];
                    PortionSize dbPortionSize = context.PortionSizes.First(p => p.Position == 0);

                    foreach (Customer customer in context.Customers.Include(cmp => cmp.CustomerMenuPlans))
                    {
                        foreach (BoxContent boxContent in dbBoxContents)
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
                                    PortionSize = dbPortionSize,
                                    PortionSizeId = dbPortionSize.Id
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
                                       .Include(r => r.Customers).ThenInclude(cld => cld.CustomersLightDiets)                                       .Include(r => r.Customers).ThenInclude(a => a.Article)
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

                        int? numberOfBoxes = null;
                        if (dbFoundOverview != null)
                        {
                            numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
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
                                    dTORoutesFoodSummary.LightDietSummary.First(l => l.Id == lightDiet.LightDietId).Value += numberOfBoxes == null ? dbCustomer.DefaultNumberOfBoxes : (int)numberOfBoxes;
                                    dTOSummaryRoutesFoodSummary.LightDietSummary.First(l => l.Id == lightDiet.LightDietId).Value += numberOfBoxes == null ? dbCustomer.DefaultNumberOfBoxes : (int)numberOfBoxes;

                                }
                            }

                            foreach (CustomersMenuPlan menuPlan in dbCustomer.CustomerMenuPlans)
                            {
                                dTORoutesFoodSummary.BoxContentSummary.First(b => b.Id == menuPlan.BoxContentId).PortionSizeSummary!.First(p => p.Id == menuPlan.PortionSizeId).Value += numberOfBoxes == null ? dbCustomer.DefaultNumberOfBoxes : (int)numberOfBoxes;
                                dTOSummaryRoutesFoodSummary.BoxContentSummary.First(b => b.Id == menuPlan.BoxContentId).PortionSizeSummary!.First(p => p.Id == menuPlan.PortionSizeId).Value += numberOfBoxes == null ? dbCustomer.DefaultNumberOfBoxes : (int)numberOfBoxes; 
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

                        int? numberOfBoxes = null;
                        if (dbFoundOverview != null)
                        {
                            numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
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
                                NumberOfBoxes = numberOfBoxes == null ? dbCustomer.DefaultNumberOfBoxes : (int)numberOfBoxes,
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

        [HttpGet("GetFoodWishes", Name = "GetFoodWishes")]
        public IActionResult GetFoodWishes()
        {
            try
            {
                List<DTOFoodWish> dTOFoodWishes = mapper.Map<List<DTOFoodWish>>(context.FoodWishes.AsNoTracking());

                int i = 0;
                foreach (DTOFoodWish dTOFoodWish in dTOFoodWishes.OrderBy(d => d.Name))
                {
                    dTOFoodWish.Position = i;
                    i++;
                }

                return Ok(dTOFoodWishes);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map food wishes");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting food wishes failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateFoodWishes", Name = "UpdateFoodWishes")]
        public IActionResult UpdateFoodWishes(List<DTOFoodWish> dTOFoodWishes)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<FoodWish> foodWishes = mapper.Map<List<FoodWish>>(dTOFoodWishes);

                var existingFoodWishes = context.FoodWishes.ToDictionary(a => a.Id);
                foreach (var foodWish in foodWishes)
                {
                    if (existingFoodWishes.TryGetValue(foodWish.Id, out var foundFoodWishes))
                    {
                        foundFoodWishes.Name = foodWish.Name;
                    }
                    else
                    {
                        context.FoodWishes.Add(foodWish);
                    }
                }

                var foodWishIds = new HashSet<int>(foodWishes.Select(a => a.Id));
                List<FoodWish> toRemove = [.. context.FoodWishes.Where(er => !foodWishIds.Contains(er.Id))];

                const int batchSize = 1000;
                for (int j = 0; j < toRemove.Count; j += batchSize)
                {
                    var batch = toRemove.Skip(j).Take(batchSize).ToList();
                    context.BulkDelete(batch);
                }

                context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map food wishes");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating food wishes failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
