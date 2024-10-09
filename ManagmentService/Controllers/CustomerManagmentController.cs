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
                Customer? customer = context.Customer.Where(c => c.Id == dTOSelectedCustomer.Id)
                                        .Include(w => w.Workdays).Include(h => h.Holidays)
                                        .Include(m => m.MonthlyOverviews).ThenInclude(d => d.DailyOverviews)
                                        .First();
                if (customer == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(mapper.Map<DTOCustomer>(customer));
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
                Customer customer = mapper.Map<Customer>(dTOCustomer);

                if (customer.Id == 0)
                {
                    if (customer.RouteId != null)
                    {
                        customer.Position = context.Customer.Where(x => x.RouteId == customer.RouteId).Count();
                    }
                    context.Customer.Add(customer);
                }
                else
                {
                    Customer? dbCustomer = context.Customer.Where(c => c.Id == customer.Id)
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

                    //dbCustomer.Workdays.Monday = customer.Workdays.Monday;
                    //dbCustomer.Workdays.Tuesday = customer.Workdays.Tuesday;
                    //dbCustomer.Workdays.Wednesday = customer.Workdays.Wednesday;
                    //dbCustomer.Workdays.Thursday = customer.Workdays.Thursday;
                    //dbCustomer.Workdays.Friday = customer.Workdays.Friday;
                    //dbCustomer.Workdays.Saturday = customer.Workdays.Saturday;
                    //dbCustomer.Workdays.Sunday = customer.Workdays.Sunday;
                    //dbCustomer.Holidays.Monday = customer.Holidays.Monday;
                    //dbCustomer.Holidays.Tuesday = customer.Holidays.Tuesday;
                    //dbCustomer.Holidays.Wednesday = customer.Holidays.Wednesday;
                    //dbCustomer.Holidays.Thursday = customer.Holidays.Thursday;
                    //dbCustomer.Holidays.Friday = customer.Holidays.Friday;
                    //dbCustomer.Holidays.Saturday = customer.Holidays.Saturday;
                    //dbCustomer.Holidays.Sunday = customer.Holidays.Sunday;

                    List<MonthlyOverview> newMonthlyOverviews = [];
                    foreach (MonthlyOverview monthlyOverview in customer.MonthlyOverviews)
                    {
                        monthlyOverview.CustomerId = customer.Id;

                        var dbMonthlyOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(m => m.Year == monthlyOverview.Year && m.Month == monthlyOverview.Month);

                        if (dbMonthlyOverview == null)
                        {
                            newMonthlyOverviews.Add(monthlyOverview);
                        }
                        else
                        {
                            DateTime today = DateTime.Today;
                            if (DateTimeCalc.MonthDifferenceMax1(today.Year, monthlyOverview.Year, today.Month, monthlyOverview.Month))
                            {
                                foreach (var dailyOverview in monthlyOverview.DailyOverviews)
                                {
                                    var dbDailyOverview = dbMonthlyOverview.DailyOverviews.FirstOrDefault(d => d.DayOfMonth == dailyOverview.DayOfMonth);

                                    if (dbDailyOverview != null)
                                    {
                                        dbDailyOverview.Price = dailyOverview.Price;
                                        dbDailyOverview.NumberOfBoxes = dailyOverview.NumberOfBoxes;
                                    }
                                }
                            }
                        }
                    }
                    context.MonthlyOverview.AddRange(newMonthlyOverviews);
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
                MonthlyOverview? monthlyOverview = context.MonthlyOverview.FirstOrDefault(m => m.CustomerId == monthOfTheYear.ReferenceId &&
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
