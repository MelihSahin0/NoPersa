using ManagmentService.Database;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Util;

namespace ManagmentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerManagmentController : ControllerBase
    {
        private NoPersaDbContext context;
        private readonly ILogger<CustomerManagmentController> logger;
        private readonly IMapper mapper;

        public CustomerManagmentController(ILogger<CustomerManagmentController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost("GetCustomer", Name= "GetCustomer")]
        public IActionResult GetCustomer([FromBody] DTOSelectedCustomer dTOSelectedCustomer)
        {
            try
            {
                Customer? dbCustomer = context.Customers.Where(c => c.Id == dTOSelectedCustomer.Id)
                                        .Include(w => w.Workdays).Include(h => h.Holidays)
                                        .Include(m => m.MonthlyOverviews).ThenInclude(d => d.DailyOverviews)
                                        .First();

                if (dbCustomer == null)
                {
                    return NotFound();
                }
                else
                {
                    CheckMonthlyOverview.CheckAndAdd(dbCustomer);

                    return Ok(mapper.Map<DTOCustomer>(dbCustomer));
                }
            }
            catch (ValidationException e)
            {
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
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
                    customer.Position = context.Customers.Where(x => x.RouteId == customer.RouteId).Count();
         
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

                    dbCustomer.Workdays = customer.Workdays;
                    dbCustomer.Holidays = customer.Holidays;

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
                }
                context.SaveChanges();
                transaction.Commit();

                return Ok(dTOCustomer);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e.Message);
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetCustomerDailyDelivery", Name = "GetCustomerDailyDelivery")]
        public IActionResult GetCustomerDailyDelivery([FromBody] DTOMonthOfTheYear monthOfTheYear)
        {
            try
            {
                MonthlyOverview? dbMonthlyOverview = context.MonthlyOverviews.FirstOrDefault(m => m.CustomerId == monthOfTheYear.ReferenceId &&
                                                                                           m.Year == monthOfTheYear.Year &&
                                                                                           m.Month == monthOfTheYear.Month);
                if (dbMonthlyOverview == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(mapper.Map<DTOMonthlyDelivery>(dbMonthlyOverview));
                }
            }
            catch (ValidationException e)
            {
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
