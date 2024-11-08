using AutoMapper;
using DeliveryService.Database;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.Delivery;
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
                return Ok(mapper.Map<List<DTORouteSummary>>(context.Routes.AsNoTracking().Include(r => r.Customers)));
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
        public IActionResult UpdateRoutes([FromBody] List<DTORouteSummary> dTORouteSummary)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<Route> newRoutes = [];
                List<Route> oldRoutes = [];
                foreach (var routeSummary in dTORouteSummary ?? [])
                {
                    Route route = mapper.Map<Route>(routeSummary);
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

                var oldRouteIds = oldRoutes.Select(d => d.Id).ToHashSet();
                var dbExistingRoutes = context.Routes.Where(r => oldRouteIds.Contains(r.Id)).ToList();
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
                var dbNotFoundRoutes = context.Routes.Where(er => !oldRouteIds.Contains(er.Id) && er.Id != int.MinValue).Include(r => r.Customers).ToList();
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
                for (int j = 0; j < dbNotFoundRoutes.SelectMany(r => r.Customers).ToList().Count; j += batchSize)
                {
                    var batch = dbNotFoundRoutes.SelectMany(r => r.Customers).ToList().Skip(j).Take(batchSize).ToList();
                    context.BulkUpdate(batch);
                }

                context.Routes.AddRange(newRoutes);

                context.SaveChanges();
                transaction.Commit();

                return Ok(dTORouteSummary);
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map route");
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
                List<DTORouteOverview> routes = [];

                //Without Archive
                List<Route> dbRoutes = await context.Routes.AsNoTracking().Where(r => r.Id != int.MinValue).Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                          .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                          .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month))
                                                                    .ThenInclude(d=>d.DailyOverviews.Where(x => x.DayOfMonth == dTOSelectedDay.Day))
                                          .ToListAsync();
                Holiday? holiday = await context.Holidays.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == dTOSelectedDay.Year && h.Month == dTOSelectedDay.Month && h.Day == dTOSelectedDay.Day);

                foreach (Route dbRoute in dbRoutes)
                {
                    List<DTOCustomerDeliveryStatus> dTOCustomerDeliveryStatusList = [];
                    foreach (Customer dbCustomer in dbRoute.Customers ?? [])
                    {
                        DTOCustomerDeliveryStatus dTOCustomerDeliveryStatus = mapper.Map<DTOCustomerDeliveryStatus>(dbCustomer);

                        MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == dTOSelectedDay.Year && x.Month == dTOSelectedDay.Month);
                     
                        if (dbFoundOverview != null)
                        {
                            int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == dTOSelectedDay.Day)).NumberOfBoxes;
                            dTOCustomerDeliveryStatus.ToDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day));
                        }
                        else //Safety Measure
                        {
                            dTOCustomerDeliveryStatus.ToDeliver = CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, dTOSelectedDay.Year, dTOSelectedDay.Month, dTOSelectedDay.Day);
                        }

                        dTOCustomerDeliveryStatusList.Add(dTOCustomerDeliveryStatus);
                    }
                    DTORouteOverview dTORouteOverview = mapper.Map<DTORouteOverview>(dbRoute);
                    dTORouteOverview.CustomerDeliveryStatus = [..dTOCustomerDeliveryStatusList];
                    routes.Add(dTORouteOverview);
                }

                return Ok(routes);
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

                return Ok(mapper.Map<List<DTOCustomersInRoute>>(dbRoutes));
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
        public IActionResult UpdateCustomerSequence([FromBody] List<DTOCustomersInRoute> dTOCustomersInRoutes)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<Customer> customersToUpdate = [];
                foreach (DTOCustomersInRoute dTOCustomerInRoute in dTOCustomersInRoutes)
                {
                    foreach (DTOCustomerSequence dTOCustomerSequence in dTOCustomerInRoute.CustomerSequence ?? [])
                    {
                        Customer? dbCustomer = context.Customers.FirstOrDefault(c => c.Id == dTOCustomerSequence.Id);

                        if (dbCustomer != null)
                        {
                            //Check if customer was taken out from Archive
                            if (dTOCustomerInRoute.Id != int.MinValue && dbCustomer.RouteId == int.MinValue)
                            {
                                CheckMonthlyOverview.CheckAndAdd(dbCustomer);
                            }

                            dbCustomer.RouteId = dTOCustomerInRoute.Id;
                            dbCustomer.Position = dTOCustomerSequence.Position;
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
