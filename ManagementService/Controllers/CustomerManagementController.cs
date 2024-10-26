using ManagementService.Database;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Util;
using System.Security.Cryptography.Xml;

namespace ManagementService.Controllers
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

        [HttpPost("GetCustomer", Name = "GetCustomer")]
        public IActionResult GetCustomer([FromBody] DTOSelectedCustomer dTOSelectedCustomer)
        {
            try
            {
                Customer? dbCustomer = context.Customers.AsNoTracking().Where(c => c.Id == dTOSelectedCustomer.Id)
                                        .Include(w => w.Workdays).Include(h => h.Holidays)
                                        .Include(m => m.MonthlyOverviews.Where(n => n.Year == DateTime.Today.Year && n.Month == DateTime.Today.Month)).ThenInclude(d => d.DailyOverviews)
                                        .Include(cld => cld.CustomersLightDiets).ThenInclude(ld => ld.LightDiet)
                                        .First();
                if (dbCustomer != null)
                {
                    DTOCustomer dTOCustomer = mapper.Map<DTOCustomer>(dbCustomer);
                    if (dTOCustomer.LightDietOverviews?.Count != context.LightDiets.AsNoTracking().Count())
                    {
                        foreach (LightDiet lightDiet in context.LightDiets.AsNoTracking().ToList())
                        {
                            if (!dTOCustomer.LightDietOverviews!.Any(ld => ld.Id == lightDiet.Id))
                            {
                                dTOCustomer.LightDietOverviews!.Add(new()
                                {
                                    Id = lightDiet.Id,
                                    Name = lightDiet.Name,
                                    Selected = false
                                });
                            }
                        }
                    }

                    return Ok(dTOCustomer);
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

        [HttpPost("UpdateCustomer", Name = "UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] DTOCustomer dTOCustomer)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                DateTime today = DateTime.Today;
                Customer customer = mapper.Map<Customer>(dTOCustomer);

                if (customer.Id == 0)
                {
                    customer.Position = (context.Customers.Where(c => c.RouteId == dTOCustomer.RouteId)
                                                          .Select(c => (int?)c.Position)
                                                          .Max() ?? -1) + 1;

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

                    context.Customers.Add(customer);
                }
                else
                {
                    Customer? dbCustomer = context.Customers.Where(c => c.Id == customer.Id)
                                           .Include(w => w.Workdays).Include(h => h.Holidays)
                                           .Include(m => m.MonthlyOverviews).ThenInclude(d => d.DailyOverviews)
                                           .Include(cld => cld.CustomersLightDiets)
                                           .FirstOrDefault();

                    if (dbCustomer == null)
                    {
                        return NotFound();
                    }

                    dbCustomer.SerialNumber = customer.SerialNumber;
                    dbCustomer.Title = customer.Title;
                    dbCustomer.Name = customer.Name;
                    dbCustomer.Address = customer.Address;
                    dbCustomer.Region = customer.Region;
                    dbCustomer.GeoLocation = customer.GeoLocation;
                    dbCustomer.ContactInformation = customer.ContactInformation;
                    dbCustomer.Article = customer.Article;
                    dbCustomer.DefaultPrice = customer.DefaultPrice;
                    dbCustomer.DefaultNumberOfBoxes = customer.DefaultNumberOfBoxes;
                    dbCustomer.TemporaryDelivery = customer.TemporaryDelivery;
                    dbCustomer.TemporaryNoDelivery = customer.TemporaryNoDelivery;

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
                            if (DateTimeCalc.MonthDifferenceMax1(today.Year, monthlyOverview.Year, today.Month, monthlyOverview.Month))
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
                    }
                    context.MonthlyOverviews.AddRange(newMonthlyOverviews);

                    foreach (CustomersLightDiet customersLightDiet in customer.CustomersLightDiets)
                    {
                        CustomersLightDiet? dbCustomersLightDiet = dbCustomer.CustomersLightDiets.FirstOrDefault(cld => cld.LightDietId == customersLightDiet.LightDietId);

                        if (dbCustomersLightDiet != null)
                        {
                            dbCustomersLightDiet.Selected = customersLightDiet.Selected;
                        }
                        else
                        {
                            customersLightDiet.Customer = customer;
                            customersLightDiet.CustomerId = customer.Id;
                            customersLightDiet.LightDiet = context.LightDiets.FirstOrDefault(d => d.Id == customersLightDiet.LightDietId)!;
                            dbCustomer.CustomersLightDiets.Add(customersLightDiet);
                        }
                    }
                }
                context.SaveChanges();
                transaction.Commit();

                return Ok(dTOCustomer);
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
                DTORoutes dTORoutes = new()
                {
                    RouteOverview = [.. mapper.Map<List<DTORouteOverview>>(context.Routes.AsNoTracking().Include(r => r.Customers))]
                };

                return Ok(dTORoutes);
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
        public IActionResult GetCustomerDailyDelivery([FromBody] DTOMonthOfTheYear monthOfTheYear)
        {
            try
            {
                MonthlyOverview? monthlyOverview = context.MonthlyOverviews.AsNoTracking().FirstOrDefault(m => m.CustomerId == monthOfTheYear.ReferenceId &&
                                                                                                          m.Year == monthOfTheYear.Year &&
                                                                                                          m.Month == monthOfTheYear.Month);
                if (monthlyOverview == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(mapper.Map<DTOMonthlyDelivery>(monthlyOverview));
                }
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

        [HttpPost("GetLightDiets", Name = "GetLightDiets")]
        public IActionResult GetLightDiets()
        {
            try
            {
                List<DTOLightDietOverview> dTOLightDietOverview = [];

                foreach (LightDiet lightDiet in context.LightDiets.AsNoTracking().ToList())
                {
                    dTOLightDietOverview.Add(new()
                    {
                        Id = lightDiet.Id,
                        Name = lightDiet.Name,
                        Selected = false
                    });
                }

                return Ok(dTOLightDietOverview);
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
    }
}