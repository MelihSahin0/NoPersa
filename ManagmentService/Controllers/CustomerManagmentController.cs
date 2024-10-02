using ManagmentService.Database;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.DTOs;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("AddCustomer", Name = "AddCustomer")]
        public IActionResult AddCustomer([FromBody] DTOCustomer customer)
        {
            try
            {
                Customer dbCustomer = mapper.Map<Customer>(customer);

                if (customer.Id == 0)
                {
                    context.Customer.Add(dbCustomer);
                }
                else
                {
                    var newMonthlyOverviews = new List<MonthlyOverview>();
                    foreach (MonthlyOverview monthlyOverview in dbCustomer.MonthlyOverviews)
                    {
                        if (!context.MonthlyOverview.Any(m => m.CustomerId == monthlyOverview.CustomerId &&
                                                              m.Year == monthlyOverview.Year &&
                                                              m.Month == monthlyOverview.Month))
                        {
                            newMonthlyOverviews.Add(monthlyOverview);
                        }
                    }

                    if (newMonthlyOverviews.Count != 0)
                    {
                        context.MonthlyOverview.AddRange(newMonthlyOverviews);
                        context.SaveChanges();
                    }

                    context.Customer.Update(dbCustomer);
                }
                context.SaveChanges();

                return Ok(customer);
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

        [HttpPost("GetCustomerDailyDelivery", Name = "GetCustomerDailyDelivery")]
        public IActionResult GetCustomerDailyDelivery([FromBody] DTOMonthOfTheYear monthOfTheYear)
        {
            try
            {
                var query = context.MonthlyOverview.AsQueryable();

                for (int i = 1; i <= 31; i++)
                {
                    var propertyName = $"Day{i}";
                    query = query.Include(propertyName);
                }

                MonthlyOverview? monthlyOverview = query.FirstOrDefault(m => m.CustomerId == monthOfTheYear.ReferenceId &&
                                                                             m.Year == int.Parse(monthOfTheYear.Year!) &&
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
