using ManagmentService.Database;
using ManagmentService.DTOs;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

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
            try
            {
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
                        Workdays = new Weekdays()
                        {
                            Monday = customer.Workdays?.Monday ?? false,
                            Tuesday = customer.Workdays?.Tuesday ?? false,
                            Wednesday = customer.Workdays?.Wednesday ?? false,
                            Thursday = customer.Workdays?.Thursday ?? false,
                            Friday = customer.Workdays?.Friday ?? false,
                            Saturday = customer.Workdays?.Saturday ?? false,
                            Sunday = customer.Workdays?.Sunday ?? false,
                        },
                        Holidays = new Weekdays()
                        {
                            Monday = customer.Holidays?.Monday ?? false,
                            Tuesday = customer.Holidays?.Tuesday ?? false,
                            Wednesday = customer.Holidays?.Wednesday ?? false,
                            Thursday = customer.Holidays?.Thursday ?? false,
                            Friday = customer.Holidays?.Friday ?? false,
                            Saturday = customer.Holidays?.Saturday ?? false,
                            Sunday = customer.Holidays?.Sunday ?? false,
                        }
                    });

                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return ValidationProblem(e.Message);
                }

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest("");
            }
        }
    }
}
