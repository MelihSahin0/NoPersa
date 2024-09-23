using ManagmentService.Database;
using ManagmentService.DTOs;
using ManagmentService.Util;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ManagmentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerManagmentController : ControllerBase
    {
        private NoPersaDbContext context;
        private readonly ILogger<CustomerManagmentController> logger;

        public CustomerManagmentController(ILogger<CustomerManagmentController> logger, NoPersaDbContext noPersaDbContext)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
        }

        [HttpPost("CustomerAdd", Name = "CustomerAdd")]
        public IActionResult CustomerAdd([FromBody] DTOCustomer customer)
        {
            WeekdaysFinder.NewWeekdays(context, customer.Workdays, customer.Holidays, out Weekdays workdays, out Weekdays holidays);

            try
            {
                context.Customer.Add(new()
                {
                    SerialNumber = customer.SerialNumber,
                    Title = customer.Title,
                    Name = customer.Name!,
                    Address = customer.Address!,
                    Region = customer.Region!,
                    GeoLocation = customer.GeoLocation,
                    ContactInformation = customer.ContactInformation,
                    Workdays = workdays,
                    Holidays = holidays,
                    Article = int.Parse(customer.Article),
                    DefaultPrice = int.Parse(customer.DefaultPrice),
                    DefaultNumberOfBoxes = int.Parse(customer.DefaultNumberOfBoxes),
                });
                context.SaveChanges();

                return Ok();
            }
            catch (ValidationException e)
            {
                logger.LogError(e.Message);
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest("");
            }
        }
    }
}
