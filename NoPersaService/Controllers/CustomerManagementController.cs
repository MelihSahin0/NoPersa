using Microsoft.AspNetCore.Mvc;
using NoPersaService.Models;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoPersaService.Util;
using NoPersaService.Database;
using SharedLibrary.Util;
using NoPersaService.DTOs.General.Answer;
using AutoMapper.QueryableExtensions;
using NoPersaService.DTOs.General.Mapped;
using NoPersaService.DTOs.General.Received;
using NoPersaService.DTOs.Management.RA;
using NoPersaService.DTOs.Management.Mapped;

namespace NoPersaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<CustomerManagementController> logger;
        private readonly IMapper mapper;

        public CustomerManagementController(ILogger<CustomerManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet("GetAllCustomersName", Name = "GetAllCustomersName")]
        public IActionResult GetAllCustomersName()
        {
            try 
            {
                var mappedIDString = context.Customers.AsNoTracking().ProjectTo<MappedIDString>(mapper.ConfigurationProvider).ToList();
                return Ok(mapper.Map<List<DTOIDString>>(mappedIDString));
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map customer");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed getting customer");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetCustomer", Name = "GetCustomer")]
        public IActionResult GetCustomer([FromBody] DTOId dTOId)
        {
            try
            {
                var mappedId = mapper.Map<MappedId>(dTOId);

                Customer? dbCustomer = context.Customers.AsNoTracking().Where(c => c.Id == mappedId.Id)
                                        .Include(dl => dl.DeliveryLocation)
                                        .Include(w => w.Workdays).Include(h => h.Holidays)
                                        .Include(m => m.MonthlyOverviews.Where(n => n.Year == DateTime.Today.Year && n.Month == DateTime.Today.Month)).ThenInclude(d => d.DailyOverviews)
                                        .Include(a => a.Article)
                                        .First();

                if (dbCustomer != null)
                {
                    CheckMonthlyOverview.CheckAndAdd(dbCustomer);

                    DTOCustomerOverview dTOCustomerOverview = mapper.Map<DTOCustomerOverview>(dbCustomer);
                    return Ok(dTOCustomerOverview);
                }

                return NotFound();
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map customer");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed getting customer");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("InsertCustomer", Name = "InsertCustomer")]
        public IActionResult InsertCustomer([FromBody] DTOCustomerOverview dTOCustomerOverview)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                dTOCustomerOverview.LightDietOverviews = dTOCustomerOverview.LightDietOverviews?.Where(ldo => ldo.Selected).ToList();
                dTOCustomerOverview.FoodWishesOverviews = dTOCustomerOverview.FoodWishesOverviews?.Where(fwo => fwo.Selected).ToList();
                dTOCustomerOverview.IngredientWishesOverviews = dTOCustomerOverview.IngredientWishesOverviews?.Where(fwo => fwo.Selected).ToList();

                DateTime today = DateTime.Today;
                Customer customer = mapper.Map<Customer>(dTOCustomerOverview);

                customer.Position = (context.Customers.Where(c => c.RouteId == customer.RouteId)
                                                          .Select(c => (int?)c.Position)
                                                          .Max() ?? -1) + 1;

                customer.Article = context.Articles.First(a => a.Id == customer.ArticleId);

                foreach (MonthlyOverview monthlyOverview in customer.MonthlyOverviews)
                {
                    if (!DateTimeCalc.MonthDifferenceMax1(today.Year, monthlyOverview.Year, today.Month, monthlyOverview.Month))
                    {
                        customer.MonthlyOverviews.Remove(monthlyOverview);
                    }
                }

                foreach (CustomersLightDiet customersLightDiet in customer.CustomersLightDiets)
                {
                    LightDiet dbLightDiet = context.LightDiets.First(d => d.Id == customersLightDiet.LightDietId);

                    customersLightDiet.CustomerId = customer.Id;
                    customersLightDiet.Customer = customer;
                    customersLightDiet.LightDietId = dbLightDiet.Id;
                    customersLightDiet.LightDiet = dbLightDiet;
                }

                foreach (CustomersFoodWish customersFoodWish in customer.CustomersFoodWish)
                {
                    FoodWish dbFoodWish = context.FoodWishes.First(d => d.Id == customersFoodWish.FoodWishId);

                    customersFoodWish.CustomerId = customer.Id;
                    customersFoodWish.Customer = customer;
                    customersFoodWish.FoodWishId = dbFoodWish.Id;
                    customersFoodWish.FoodWish = dbFoodWish;
                }

                foreach (CustomersMenuPlan customerMenuPlan in customer.CustomerMenuPlans)
                {
                    customerMenuPlan.Customer = customer;
                    customerMenuPlan.CustomerId = customer.Id;
                    customerMenuPlan.BoxContentId = customerMenuPlan.BoxContentId;
                    customerMenuPlan.BoxContent = context.BoxContents.FirstOrDefault(p => p.Id == customerMenuPlan.BoxContentId)!;
                    customerMenuPlan.PortionSizeId = customerMenuPlan.PortionSizeId;
                    customerMenuPlan.PortionSize = context.PortionSizes.FirstOrDefault(p => p.Id == customerMenuPlan.PortionSizeId)!;
                }

                context.Customers.Add(customer);

                context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map customer");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Failed updating customer");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateCustomer", Name = "UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] DTOCustomerOverview dTOCustomerOverview)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                dTOCustomerOverview.LightDietOverviews = dTOCustomerOverview.LightDietOverviews?.Where(ldo => ldo.Selected).ToList();
                dTOCustomerOverview.FoodWishesOverviews = dTOCustomerOverview.FoodWishesOverviews?.Where(fwo => fwo.Selected).ToList();
                dTOCustomerOverview.IngredientWishesOverviews = dTOCustomerOverview.IngredientWishesOverviews?.Where(fwo => fwo.Selected).ToList();

                DateTime today = DateTime.Today;
                Customer customer = mapper.Map<Customer>(dTOCustomerOverview);

                Customer? dbCustomer = context.Customers.Where(c => c.Id == customer.Id)
                                       .Include(dl => dl.DeliveryLocation)
                                       .Include(w => w.Workdays).Include(h => h.Holidays)
                                       .Include(m => m.MonthlyOverviews).ThenInclude(d => d.DailyOverviews)
                                       .Include(cld => cld.CustomersLightDiets)
                                       .Include(cmp => cmp.CustomerMenuPlans)
                                       .Include(cfw => cfw.CustomersFoodWish)
                                       .Include(a => a.Article)
                                       .FirstOrDefault();

                if (dbCustomer == null)
                {
                    return NotFound();
                }

                dbCustomer.SerialNumber = customer.SerialNumber;
                dbCustomer.Title = customer.Title;
                dbCustomer.Name = customer.Name;
                dbCustomer.ContactInformation = customer.ContactInformation;
                dbCustomer.DefaultNumberOfBoxes = customer.DefaultNumberOfBoxes;
                dbCustomer.TemporaryDelivery = customer.TemporaryDelivery;
                dbCustomer.TemporaryNoDelivery = customer.TemporaryNoDelivery;

                dbCustomer.DeliveryLocation!.Address = customer.DeliveryLocation!.Address;
                dbCustomer.DeliveryLocation.Region = customer.DeliveryLocation.Region;
                dbCustomer.DeliveryLocation.Latitude = customer.DeliveryLocation.Latitude;
                dbCustomer.DeliveryLocation.Longitude = customer.DeliveryLocation.Longitude;
                dbCustomer.DeliveryLocation.DeliveryWhishes = customer.DeliveryLocation.DeliveryWhishes;

                dbCustomer.ArticleId = customer.ArticleId;
                dbCustomer.Article = context.Articles.First(a => a.Id == customer.ArticleId);

                dbCustomer.Workdays.Monday = customer.Workdays.Monday;
                dbCustomer.Workdays.Tuesday = customer.Workdays.Tuesday;
                dbCustomer.Workdays.Wednesday = customer.Workdays.Wednesday;
                dbCustomer.Workdays.Thursday = customer.Workdays.Thursday;
                dbCustomer.Workdays.Friday = customer.Workdays.Friday;
                dbCustomer.Workdays.Saturday = customer.Workdays.Saturday;
                dbCustomer.Workdays.Sunday = customer.Workdays.Sunday;
                dbCustomer.Holidays.Monday = customer.Holidays.Monday;
                dbCustomer.Holidays.Tuesday = customer.Holidays.Tuesday;
                dbCustomer.Holidays.Wednesday = customer.Holidays.Wednesday;
                dbCustomer.Holidays.Thursday = customer.Holidays.Thursday;
                dbCustomer.Holidays.Friday = customer.Holidays.Friday;
                dbCustomer.Holidays.Saturday = customer.Holidays.Saturday;
                dbCustomer.Holidays.Sunday = customer.Holidays.Sunday;

                #region Monthly
                List<MonthlyOverview> overviewsToRemove = [];
                foreach (MonthlyOverview monthlyOverview in customer.MonthlyOverviews)
                {
                    if (!DateTimeCalc.MonthDifferenceMax1(today.Year, monthlyOverview.Year, today.Month, monthlyOverview.Month))
                    {
                        overviewsToRemove.Add(monthlyOverview);
                    }
                }

                foreach (MonthlyOverview overviewToRemove in overviewsToRemove)
                {
                    customer.MonthlyOverviews.Remove(overviewToRemove);
                }

                List<MonthlyOverview> newMonthlyOverviews = [];
                foreach (MonthlyOverview monthlyOverview in customer.MonthlyOverviews)
                {
                    monthlyOverview.CustomerId = customer.Id;

                    MonthlyOverview? dbMonthlyOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(m => m.Year == monthlyOverview.Year && m.Month == monthlyOverview.Month);

                    if (dbMonthlyOverview == null)
                    {
                        newMonthlyOverviews.Add(monthlyOverview);
                    }
                    else
                    {
                        foreach (DailyOverview dailyOverview in monthlyOverview.DailyOverviews)
                        {
                            DailyOverview? dbDailyOverview = dbMonthlyOverview.DailyOverviews.FirstOrDefault(d => d.DayOfMonth == dailyOverview.DayOfMonth);

                            if (dbDailyOverview != null)
                            {
                                dbDailyOverview.Price = dailyOverview.Price;
                                dbDailyOverview.NumberOfBoxes = dailyOverview.NumberOfBoxes;
                            }
                        }                        
                    }
                }
                context.MonthlyOverviews.AddRange(newMonthlyOverviews);
                #endregion

                #region lightDiets
                var existingLightDiet = dbCustomer.CustomersLightDiets.ToDictionary(cl => cl.LightDietId);
                foreach (var lightDiet in customer.CustomersLightDiets)
                {
                    if (!existingLightDiet.TryGetValue(lightDiet.LightDietId, out var foundLightDiet))
                    {
                        LightDiet dbLightDiet = context.LightDiets.First(d => d.Id == lightDiet.LightDietId);

                        lightDiet.CustomerId = dbCustomer.Id;
                        lightDiet.Customer = dbCustomer;
                        lightDiet.LightDietId = dbLightDiet.Id;
                        lightDiet.LightDiet = dbLightDiet;
                        dbCustomer.CustomersLightDiets.Add(lightDiet);
                    }
                }

                var lightDietsIds = new HashSet<long>(customer.CustomersLightDiets.Select(a => a.LightDietId));
                List<CustomersLightDiet> toRemoveLightDiets = [..dbCustomer.CustomersLightDiets.Where(er => !lightDietsIds.Contains(er.LightDietId))];
                foreach (var toRemoveLightDiet in toRemoveLightDiets)
                {
                    dbCustomer.CustomersLightDiets.Remove(toRemoveLightDiet);
                }
                #endregion

                #region foodwishes
                var existingFoodWishes = dbCustomer.CustomersFoodWish.ToDictionary(cl => cl.FoodWishId);
                foreach (var foodWish in customer.CustomersFoodWish)
                {
                    if (!existingFoodWishes.TryGetValue(foodWish.FoodWishId, out var foundFoodWish))
                    {
                        FoodWish dbFoodWish = context.FoodWishes.First(d => d.Id == foodWish.FoodWishId);

                        foodWish.CustomerId = dbCustomer.Id;
                        foodWish.Customer = dbCustomer;
                        foodWish.FoodWishId = dbFoodWish.Id;
                        foodWish.FoodWish = dbFoodWish;
                        dbCustomer.CustomersFoodWish.Add(foodWish);
                    }
                }

                var foodWishesIds = new HashSet<long>(customer.CustomersFoodWish.Select(a => a.FoodWishId));
                List<CustomersFoodWish> toRemoveFoodWishes = [.. dbCustomer.CustomersFoodWish.Where(er => !foodWishesIds.Contains(er.FoodWishId))];
                foreach (var toRemoveFooDWish in toRemoveFoodWishes)
                {
                    dbCustomer.CustomersFoodWish.Remove(toRemoveFooDWish);
                }
                #endregion

                foreach (CustomersMenuPlan customerMenuPlan in customer.CustomerMenuPlans)
                {
                    CustomersMenuPlan? dbCustomerMenuPlan = dbCustomer.CustomerMenuPlans.FirstOrDefault(cmp => cmp.BoxContentId == customerMenuPlan.BoxContentId);

                    if (dbCustomerMenuPlan != null)
                    {
                        dbCustomerMenuPlan.PortionSize = context.PortionSizes.FirstOrDefault(p => p.Id == customerMenuPlan.PortionSizeId)!; ;
                        dbCustomerMenuPlan.PortionSizeId = customerMenuPlan.PortionSizeId;
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map customer");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Failed updating customer");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpGet("GetRoutesOverview", Name = "GetRoutesOverview")]
        public IActionResult GetRoutesOverview()
        {
            try
            {
                return Ok(context.Routes.AsNoTracking().ProjectTo<DTOSelectInput>(mapper.ConfigurationProvider).ToList());
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

        [HttpPost("GetCustomerDailyDelivery", Name = "GetCustomerDailyDelivery")]
        public IActionResult GetCustomerDailyDelivery([FromBody] DTOMonthOfTheYear dTOMonthOfTheYear)
        {
            try
            {
                var mappedMonthOfTheYear = mapper.Map<MappedMonthOfTheYear>(dTOMonthOfTheYear);

                MonthlyOverview? monthlyOverview = context.MonthlyOverviews.AsNoTracking().Include(d => d.DailyOverviews).FirstOrDefault(m => m.CustomerId == mappedMonthOfTheYear.ReferenceId &&
                                                                                                                                         m.Year == mappedMonthOfTheYear.Year &&
                                                                                                                                         m.Month == mappedMonthOfTheYear.Month);
               
                if (monthlyOverview == null)
                {
                    monthlyOverview = CheckMonthlyOverview.Generate(null, new DateTime(mappedMonthOfTheYear.Year, mappedMonthOfTheYear.Month, DateTime.DaysInMonth(mappedMonthOfTheYear.Year, mappedMonthOfTheYear.Month)));
                }
                
                return Ok(mapper.Map<DTOMonthlyDelivery>(monthlyOverview));
                
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map customer daily delivery");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "failed getting customer daily delivery");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetGastro", Name = "GetGastro")]
        public IActionResult GetGastro([FromBody] DTOId dTOId)
        {
            try
            {
                var mappedId = mapper.Map<MappedId>(dTOId);

                DTOCustomersGastro dTOCustomersGastro = new();

                #region lightDiet
                var dTOLightDietOverview = context.LightDiets.AsNoTracking().ProjectTo<DTOLightDietOverview>(mapper.ConfigurationProvider).ToList();

                if (mappedId.Id != null)
                {
                    HashSet<long> selectedIds = new(context.CustomersLightDiets.AsNoTracking().Where(cld => cld.CustomerId == mappedId.Id).Select(x => x.LightDietId));
                    foreach (var lightDiet in dTOLightDietOverview)
                    {
                        lightDiet.Selected = selectedIds.Contains((long)IdEncryption.DecryptId(lightDiet.Id)!);
                    }
                }
                dTOCustomersGastro.LightDietOverview = [.. dTOLightDietOverview.OrderBy(x => x.Position)];
                #endregion

                #region foodWishes
                var foodWishes = context.FoodWishes.AsNoTracking().ProjectTo<DTOFoodWishesOverview>(mapper.ConfigurationProvider).ToList();

                var dTOFoodWishes = foodWishes.Where(f => !f.IsIngredient).ToList();
                var dTOIngredientWishes = foodWishes.Where(f => f.IsIngredient).ToList();

                if (mappedId.Id != null)
                {
                    HashSet<long> selectedIds = new(context.CustomersFoodWishes.AsNoTracking().Where(cld => cld.CustomerId == mappedId.Id).Select(x => x.FoodWishId));
                    foreach (var foodWish in dTOFoodWishes)
                    {
                        foodWish.Selected = selectedIds.Contains((long)IdEncryption.DecryptId(foodWish.Id)!);
                    }
                    foreach (var ingredientWish in dTOIngredientWishes)
                    {
                        ingredientWish.Selected = selectedIds.Contains((long)IdEncryption.DecryptId(ingredientWish.Id)!);
                    }
                }
                dTOCustomersGastro.FoodWishesOverviews = [.. dTOFoodWishes.OrderBy(x => x.Position)];
                dTOCustomersGastro.IngredientWishesOverviews = [.. dTOIngredientWishes.OrderBy(x => x.Position)];
                #endregion

                #region boxContent
                if (!context.BoxContents.AsNoTracking().Any() || !context.PortionSizes.AsNoTracking().Any())
                {
                    return NotFound("At least one box content and one portion size is required");
                }

                List<PortionSize> portionSizes = [.. context.PortionSizes.AsNoTracking()];
                List<DTOBoxContentSelected> dTOBoxContentSelectedList = [];

                if (mappedId.Id == null)
                {
                    var id = IdEncryption.EncryptId(portionSizes.First(ps => ps.IsDefault).Id);
                    dTOBoxContentSelectedList = [.. context.BoxContents.AsNoTracking().Select(boxContent => new DTOBoxContentSelected
                    {
                        Id = IdEncryption.EncryptId(boxContent.Id),
                        Position = boxContent.Position,
                        Name = boxContent.Name,
                        PortionSizeId = id
                    })];
                }
                else
                {
                    dTOBoxContentSelectedList = mapper.Map<List<DTOBoxContentSelected>>(context.CustomersMenuPlans.AsNoTracking().Where(cmp => cmp.CustomerId == mappedId.Id).Include(b => b.BoxContent).Include(p => p.PortionSize));
                }

                dTOCustomersGastro.BoxContentSelectedList = [.. dTOBoxContentSelectedList.OrderBy(x => x.Position)];
                dTOCustomersGastro.SelectInputs = [.. mapper.Map<List<DTOSelectInput>>(portionSizes).OrderBy(x => x.Position)];
                #endregion

                return Ok(dTOCustomersGastro);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map customers gastronomy");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "failed getting customers gastronomy");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}