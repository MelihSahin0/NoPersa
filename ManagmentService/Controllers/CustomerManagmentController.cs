using ManagmentService.Database;
using ManagmentService.DTOs;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;
using SharedLibrary.Validations;
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
            if (customer.Id == -1)
            {
                return AddCustomer(customer);
            }

            //TODO
            return Ok("");
        }

        private IActionResult AddCustomer(DTOCustomer customer)
        {
            try
            {
                Weekdays workdays = new()
                {
                    Monday = customer.Workdays?.Monday ?? false,
                    Tuesday = customer.Workdays?.Tuesday ?? false,
                    Wednesday = customer.Workdays?.Wednesday ?? false,
                    Thursday = customer.Workdays?.Thursday ?? false,
                    Friday = customer.Workdays?.Friday ?? false,
                    Saturday = customer.Workdays?.Saturday ?? false,
                    Sunday = customer.Workdays?.Sunday ?? false,
                };
                Weekdays holidays = new()
                {
                    Monday = customer.Holidays?.Monday ?? false,
                    Tuesday = customer.Holidays?.Tuesday ?? false,
                    Wednesday = customer.Holidays?.Wednesday ?? false,
                    Thursday = customer.Holidays?.Thursday ?? false,
                    Friday = customer.Holidays?.Friday ?? false,
                    Saturday = customer.Holidays?.Saturday ?? false,
                    Sunday = customer.Holidays?.Sunday ?? false,
                };
                context.Weekdays.Add(workdays);
                context.Weekdays.Add(holidays);
                context.SaveChanges();

                Customer dbCustomer = new()
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
                };
                context.Customer.Add(dbCustomer);
                context.SaveChanges();

                List<MonthlyOverview> monthlyOverviews = [];
                foreach (DTOMonthlyDelivery monthlyDelivery in customer.MonthlyDeliveries ?? [])
                {
                    List<DailyOverview> dailyOverviews = [];

                    for (int i = 1; i <= 31; i++)
                    {
                        string dayPropertyName = $"Day{i}";

                        Parser.ParseToDouble(((DTODailyDelivery)monthlyDelivery.GetType().GetProperty(dayPropertyName)?.GetValue(monthlyDelivery))?.Price ?? "", out double result);
                        DailyOverview dailyOverview = new()
                        {
                            Price = string.IsNullOrWhiteSpace(((DTODailyDelivery)monthlyDelivery.GetType().GetProperty(dayPropertyName)?.GetValue(monthlyDelivery))?.Price) ? null : result,
                            NumberOfBoxes = string.IsNullOrWhiteSpace(((DTODailyDelivery)monthlyDelivery.GetType().GetProperty(dayPropertyName)?.GetValue(monthlyDelivery))?.NumberOfBoxes) ? null : int.Parse(((DTODailyDelivery)monthlyDelivery.GetType().GetProperty(dayPropertyName)?.GetValue(monthlyDelivery))?.NumberOfBoxes ?? "0")
                        };
                        dailyOverviews.Add(dailyOverview);
                    }
                    MonthlyOverview monthlyOverview = new()
                    {
                        Month = (int)monthlyDelivery.MonthOfTheYear?.Month!,
                        Year = int.Parse(monthlyDelivery.MonthOfTheYear?.Year!),
                        Day1 = dailyOverviews[0],
                        Day2 = dailyOverviews[1],
                        Day3 = dailyOverviews[2],
                        Day4 = dailyOverviews[3],
                        Day5 = dailyOverviews[4],
                        Day6 = dailyOverviews[5],
                        Day7 = dailyOverviews[6],
                        Day8 = dailyOverviews[7],
                        Day9 = dailyOverviews[8],
                        Day10 = dailyOverviews[9],
                        Day11 = dailyOverviews[10],
                        Day12 = dailyOverviews[11],
                        Day13 = dailyOverviews[12],
                        Day14 = dailyOverviews[13],
                        Day15 = dailyOverviews[14],
                        Day16 = dailyOverviews[15],
                        Day17 = dailyOverviews[16],
                        Day18 = dailyOverviews[17],
                        Day19 = dailyOverviews[18],
                        Day20 = dailyOverviews[19],
                        Day21 = dailyOverviews[20],
                        Day22 = dailyOverviews[21],
                        Day23 = dailyOverviews[22],
                        Day24 = dailyOverviews[23],
                        Day25 = dailyOverviews[24],
                        Day26 = dailyOverviews[25],
                        Day27 = dailyOverviews[26],
                        Day28 = dailyOverviews[27],
                        Day29 = dailyOverviews[28],
                        Day30 = dailyOverviews[29],
                        Day31 = dailyOverviews[30],
                        Customer = dbCustomer
                    };

                    context.DailyOverview.AddRange(dailyOverviews);
                    monthlyOverviews.Add(monthlyOverview);
                }
                context.MonthlyOverview.AddRange(monthlyOverviews);
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
