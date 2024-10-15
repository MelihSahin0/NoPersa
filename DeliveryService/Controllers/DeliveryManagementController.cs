using AutoMapper;
using DeliveryService.Database;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using SharedLibrary.DTOs.GetDTOs;
using SharedLibrary.Models;
using SharedLibrary.Util;
using System.ComponentModel.DataAnnotations;
using Holiday = SharedLibrary.Models.Holiday;
using Route = SharedLibrary.Models.Route;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<DeliveryManagementController> logger;
        private readonly IMapper mapper;
        private readonly string currentCountry = "at"; //TODO: Will be removed in the future

        public DeliveryManagementController(ILogger<DeliveryManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
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


        [HttpPost("UpdateRoutes", Name = "UpdateRoutes")]
        public IActionResult UpdateRoutes([FromBody] DTORoutes routes)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<Route> newRoutes = [];
                List<Route> oldRoutes = [];
                foreach (var routeDto in routes.RouteOverview ?? [])
                {
                    Route route = mapper.Map<Route>(routeDto);
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

                var dbExistingRoutes = context.Routes.Where(r => oldRoutes.Select(or => or.Id).Contains(r.Id)).ToList();
                foreach (var dbExistingRoute in dbExistingRoutes)
                {
                    var dbUpdatedRoute = oldRoutes.FirstOrDefault(or => or.Id == dbExistingRoute.Id);
                    if (dbUpdatedRoute != null)
                    {
                        dbExistingRoute.Position = dbUpdatedRoute.Position;
                        dbExistingRoute.Name = dbUpdatedRoute.Name;
                    }
                }

                //without Archive
                var dbNotFoundRoutes = context.Routes.Where(er => !oldRoutes.Select(or => or.Id).Contains(er.Id) && er.Id != int.MinValue).Include(r => r.Customers).ToList();
                int i = (context.Customers.Where(c => c.RouteId == int.MinValue)
                                          .Select(c => (int?)c.Position)
                                          .Max() ?? -1) + 1;
                foreach (var dbObsoleteRoute in dbNotFoundRoutes)
                {
                    foreach (Customer dbCustomer in dbObsoleteRoute.Customers)
                    {
                        dbCustomer.RouteId = int.MinValue;
                        dbCustomer.Position = i;
                        i++;
                    }
                    context.Routes.Remove(dbObsoleteRoute);
                }                

                //dbNotFoundRoutes can update many customers
                const int batchSize = 1000;
                for (int j = 0; j < dbNotFoundRoutes.Count; j += batchSize)
                {
                    var batch = dbNotFoundRoutes.Skip(j).Take(batchSize).ToList();
                    context.BulkInsertOrUpdate(batch);
                }

                context.Routes.AddRange(newRoutes);

                context.SaveChanges();
                transaction.Commit();

                return Ok(routes);
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
                logger.LogError(e, "Updating routes failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("GetDeliveryStatus", Name = "GetDeliveryStatus")]
        public async Task<IActionResult> GetDeliveryStatus([FromBody] DTOSelectedDay dTOSelectedDay)
        {
            try
            {
                DTODeliveryStatus dTODeliveryStatus = new();
                List<DTORouteDetails> routes = [];

                //Without Archive
                List<Route> dbRoutes = await context.Routes.AsNoTracking().Where(r => r.Id != int.MinValue).Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                          .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                          .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month))
                                                                    .ThenInclude(d=>d.DailyOverviews.Where(x => x.DayOfMonth == dTOSelectedDay.Day))
                                          .ToListAsync();

                foreach (Route dbRoute in dbRoutes)
                {
                    List<DTOCustomerRoute> customerRoutes = [];
                    foreach (Customer dbCustomer in dbRoute.Customers ?? [])
                    {
                        DTOCustomerRoute dTOCustomerRoutes = mapper.Map<DTOCustomerRoute>(dbCustomer);

                        MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month);
                        Holiday? holiday = await context.Holidays.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == dTOSelectedDay.Year && h.Month == dTOSelectedDay.Month && h.Day == dTOSelectedDay.Day);

                        if (dbFoundOverview != null)
                        {
                            int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
                            dTOCustomerRoutes.ToDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day));
                        }
                        else //Safety Measure
                        {
                            dTOCustomerRoutes.ToDeliver = CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day);
                        }

                        customerRoutes.Add(dTOCustomerRoutes);
                    }
                    DTORouteDetails dTORouteDetails = mapper.Map<DTORouteDetails>(dbRoute);
                    dTORouteDetails.CustomersRoute = [..customerRoutes];
                    routes.Add(dTORouteDetails);
                }

                dTODeliveryStatus.RouteDetails = [.. routes];

                return Ok(dTODeliveryStatus);
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map deliveryStatus");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting delivery status failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpGet("GetRouteDetails", Name = "GetRouteDetails")]
        public IActionResult GetRouteDetails()
        {
            try
            {
                List<Route> dbRoutes = [.. context.Routes.AsNoTracking().Include(r => r.Customers)];

                return Ok(mapper.Map<List<DTOSequenceDetails>>(dbRoutes));
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map route details");
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed getting route details");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateCustomerSequence", Name = "UpdateCustomerSequence")]
        public IActionResult UpdateCustomerSequence([FromBody] List<DTOSequenceDetails> dTOSequenceDetails)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<Customer> customersToUpdate = [];
                foreach (DTOSequenceDetails route in dTOSequenceDetails)
                {
                    foreach (DTOCustomerSequence customer in route.CustomersRoute ?? [])
                    {
                        Customer? dbCustomer = context.Customers.FirstOrDefault(c => c.Id == customer.Id);

                        if (dbCustomer != null)
                        {
                            if (route.Id == int.MinValue && dbCustomer.RouteId != route.Id)
                            {
                                CheckMonthlyOverview.CheckAndAdd(dbCustomer);
                            }

                            dbCustomer.RouteId = route.Id;
                            dbCustomer.Position = customer.Position;
                            customersToUpdate.Add(dbCustomer);
                        }
                    }
                }

                const int batchSize = 1000;
                for (int i = 0; i < customersToUpdate.Count; i += batchSize)
                {
                    var batch = customersToUpdate.Skip(i).Take(batchSize).ToList();
                    context.BulkUpdate(batch);
                }

                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map customer sequence");
                return ValidationProblem(e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Failed getting customer sequence");
                return BadRequest("An error occurred while processing your request.");
            }
        }  
    }
}
