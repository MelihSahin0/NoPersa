using AutoMapper;
using DeliveryService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.DTOs;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using Route = SharedLibrary.Models.Route;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryManagmentController : ControllerBase
    {
        private NoPersaDbContext context;
        private readonly ILogger<DeliveryManagmentController> logger;
        private readonly IMapper mapper;

        public DeliveryManagmentController(ILogger<DeliveryManagmentController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost("UpdateRoutes", Name = "UpdateRoutes")]
        public IActionResult UpdateRoutes([FromBody] DTORoutes routes)
        {
            try
            {
                List<Route> newRoutes = [];
                List<Route> oldRoutes = [];
                for (int i = 0; i < routes.RouteOverview?.Length; i++)
                {
                    Route route = mapper.Map<Route>(routes.RouteOverview[i]);
                    route.Customers = [];

                    if (route.Id == 0)
                    {
                        newRoutes.Add(route);
                    }
                    else
                    {
                        oldRoutes.Add(route);
                    }
                }

                var existingRoutes = context.Routes.Where(r => oldRoutes.Select(or => or.Id).Contains(r.Id)).ToList();
                foreach (var existingRoute in existingRoutes)
                {
                    var updatedRoute = oldRoutes.FirstOrDefault(or => or.Id == existingRoute.Id);
                    if (updatedRoute != null)
                    {
                        existingRoute.Position = updatedRoute.Position;
                        existingRoute.Name = updatedRoute.Name;
                    }
                }

                var notFoundRoutes = context.Routes.Where(er => !oldRoutes.Select(or => or.Id).Contains(er.Id)).Include(r => r.Customers).ToList();
                foreach (var obsoleteRoute in notFoundRoutes)
                {
                    foreach (Customer customer in obsoleteRoute.Customers)
                    {
                        customer.Position = null;
                    }
                    context.Routes.Remove(obsoleteRoute);
                }

                context.Routes.AddRange(newRoutes);

                context.SaveChanges();
                return Ok(routes);
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

        [HttpGet("GetRoutesOverview", Name = "GetRoutesOverview")]
        public IActionResult GetRoutesOverview()
        {
            try
            {
                DTORoutes dTORoutes = new();
                List<DTORouteOverview> routes = [];

                foreach (Route route in context.Routes.Include(r => r.Customers))
                {
                    routes.Add(mapper.Map<DTORouteOverview>(route));
                }
                dTORoutes.RouteOverview = [.. routes];

                return Ok(dTORoutes);
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

        [HttpPost("GetRoutesDetails", Name = "GetRoutesDetails")]
        public IActionResult GetRoutesDetails([FromBody] DTOSelectedDay dTOSelectedDay)
        {
            try
            {
                DTODeliveryStatus dTODeliveryStatus = new();
                List<DTORouteDetails> routes = [];

                var query = context.Routes.Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                          .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                          .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month))
                                                                    .ThenInclude(d=>d.DailyOverviews.Where(x => x.DayOfMonth == dTOSelectedDay.Day));

                foreach (Route route in query)
                {
                    List<DTOCustomerRoute> customerRoutes = [];
                    foreach (Customer customer in route.Customers ?? [])
                    {
                        DTOCustomerRoute dTOCustomerRoutes = mapper.Map<DTOCustomerRoute>(customer);

                        MonthlyOverview? foundOverview = customer.MonthlyOverviews.FirstOrDefault(x =>
                                x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month);

                        if (foundOverview != null)
                        {
                            int? numberOfBoxes = ((DailyOverview)foundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
                            dTOCustomerRoutes.ToDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && SetDeliveryToTrueOrFalse(customer, dTOSelectedDay));
                        }
                        else
                        {
                            dTOCustomerRoutes.ToDeliver = SetDeliveryToTrueOrFalse(customer, dTOSelectedDay);
                        }

                        customerRoutes.Add(dTOCustomerRoutes);
                    }
                    DTORouteDetails dTORouteDetails = mapper.Map<DTORouteDetails>(route);
                    dTORouteDetails.CustomersRoute = [..customerRoutes];
                    routes.Add(dTORouteDetails);
                }

                dTODeliveryStatus.RouteDetails = [.. routes];

                return Ok(dTODeliveryStatus);
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

        private bool SetDeliveryToTrueOrFalse(Customer customer, DTOSelectedDay dTOSelectedDay)
        {
            DateTime date = new(dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day);
            string propertyName = date.DayOfWeek.ToString();

            //Call if date is holiday
            if (true) //No Holiady
            {
                var propertyInfo = typeof(Weekdays).GetProperty(propertyName);

                if (propertyInfo == null || !propertyInfo.CanRead)
                {
                    throw new InvalidOperationException($"Property '{propertyName}' does not exist or cannot be read.");
                }

                return (bool)(propertyInfo.GetValue(customer.Workdays) ?? true);  
            }
            else
            {
                return false;   
            }
        }
    }
}
