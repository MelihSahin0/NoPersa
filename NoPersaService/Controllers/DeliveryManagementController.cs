﻿using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoPersaService.Database;
using NoPersaService.DTOs.APICalls;
using NoPersaService.DTOs.Delivery.Answer;
using NoPersaService.DTOs.Delivery.Mapped;
using NoPersaService.DTOs.Delivery.RA;
using NoPersaService.DTOs.Delivery.Receive;
using NoPersaService.DTOs.General.Received;
using NoPersaService.Models;
using NoPersaService.Util;
using System.ComponentModel.DataAnnotations;
using Holiday = NoPersaService.Models.Holiday;
using Route = NoPersaService.Models.Route;

namespace NoPersaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<DeliveryManagementController> logger;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;
        private readonly string currentCountry = "at"; //TODO: Will be removed in the future

        public DeliveryManagementController(ILogger<DeliveryManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper, HttpClient httpClient)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.httpClient = httpClient;
        }

        [HttpGet("GetRoutesSummary", Name = "GetRoutesSummary")]
        public IActionResult GetRoutesSummary()
        {
            try
            {
                return Ok(mapper.Map<List<DTORouteSummary>>(context.Routes.AsNoTracking().Where(r => r.Id != long.MinValue).Include(r => r.Customers)));
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
                List<Route> routes = mapper.Map<List<Route>>(dTORouteSummary);

                var existingRoutes = context.Routes.ToDictionary(a => a.Id);
                foreach (var route in routes)
                {
                    //update
                    if (existingRoutes.TryGetValue(route.Id, out var foundRoute))
                    {
                        foundRoute.Position = route.Position;
                        foundRoute.Name = route.Name;
                    }
                    else //insert
                    {
                        context.Routes.Add(route);
                    }
                }

                //without Archive
                var routeIds = new HashSet<long>(routes.Select(a => a.Id));
                List<Route> toRemove = [.. context.Routes.Where(er => !routeIds.Contains(er.Id) && er.Id != long.MinValue).Include(r => r.Customers)];
              
                int i = (context.Customers.Where(c => c.RouteId == long.MinValue)
                                          .Select(c => (int?)c.Position)
                                          .Max() ?? -1) + 1;
                foreach (var dbObsoleteRoute in toRemove)
                {
                    foreach (Customer dbCustomer in dbObsoleteRoute.Customers)
                    {
                        dbCustomer.RouteId = long.MinValue;
                        dbCustomer.Position = i;
                        i++;
                    }
                }

                //dbNotFoundRoutes can update many customers
                const int batchSize = 1000;
                for (int j = 0; j < toRemove.SelectMany(r => r.Customers).ToList().Count; j += batchSize)
                {
                    var batch = toRemove.SelectMany(r => r.Customers).ToList().Skip(j).Take(batchSize).ToList();
                    context.BulkUpdate(batch);
                }

                context.Routes.RemoveRange(toRemove);

                context.SaveChanges();
                transaction.Commit();

                return Ok();
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
                List<Route> dbRoutes = await context.Routes.AsNoTracking().Where(r => r.Id != long.MinValue).Include(r => r.Customers).ThenInclude(c => c.Workdays)
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
                return Ok(mapper.Map<List<DTOCustomersInRoute>>(context.Routes.AsNoTracking().Include(r => r.Customers)));
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
                var routeId = new HashSet<long>(context.Routes.AsNoTracking().Select(r => r.Id)); 
                List<Customer> customersToUpdate = [];
                foreach (DTOCustomersInRoute dTOCustomerInRoute in dTOCustomersInRoutes)
                {
                    var idCustomerInRoute = IdEncryption.DecryptId(dTOCustomerInRoute.Id);
                    var foundRouteId = routeId.Contains((long)idCustomerInRoute!);

                    if (!foundRouteId)
                    {
                        continue;
                    }

                    foreach (DTOCustomerSequence dTOCustomerSequence in dTOCustomerInRoute.CustomerSequence ?? [])
                    {
                        var idCustomerSequence = IdEncryption.DecryptId(dTOCustomerSequence.Id);

                        Customer? dbCustomer = context.Customers.Include(a => a.Article)
                                                                .Include(m => m.MonthlyOverviews).FirstOrDefault(c => c.Id == idCustomerSequence);

                        if (dbCustomer != null)
                        {
                            //Check if customer was taken out from Archive
                            if (idCustomerInRoute != long.MinValue && dbCustomer.RouteId == long.MinValue)
                            {
                                CheckMonthlyOverview.CheckAndAdd(dbCustomer);
                            }

                            dbCustomer.RouteId = (long)idCustomerInRoute!;
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

                context.SaveChanges();
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

        [HttpGet("{z}/{x}/{y}", Name = "GetTile")]
        public async Task<IActionResult> GetTile(int z, int x, int y)
        {
            string url = $"https://api.tomtom.com/map/1/tile/basic/main/{z}/{x}/{y}.png?key={Environment.GetEnvironmentVariable("ROUTING_API")}";

            try
            {
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var imageData = await response.Content.ReadAsByteArrayAsync();
                    return File(imageData, "image/png");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error fetching tile.");
                }
            }
            catch
            {
                return StatusCode(500, "Error fetching tile.");
            }
        }

        [HttpPost("GetRouting", Name = "GetRouting")]
        public async Task<IActionResult> GetRouting([FromBody] DTOSelectedDayWithReference dTOSelectedDayWithReference)
        {
            try
            {
                var mappedSelectedDayWithRef = mapper.Map<MappedSelectedDayWithReference>(dTOSelectedDayWithReference);

                Route? dbRoute = await context.Routes.AsNoTracking().Where(r => r.Id != long.MinValue && r.Id == mappedSelectedDayWithRef.ReferenceId)
                                                      .Include(r => r.Customers).ThenInclude(c => c.DeliveryLocation)
                                                      .Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                                      .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                                      .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == mappedSelectedDayWithRef.Year && x.Month == mappedSelectedDayWithRef.Month))
                                                                                .ThenInclude(d => d.DailyOverviews.Where(x => x.DayOfMonth == mappedSelectedDayWithRef.Day))
                                                      .FirstOrDefaultAsync();
                Holiday? holiday = await context.Holidays.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == mappedSelectedDayWithRef.Year && h.Month == mappedSelectedDayWithRef.Month && h.Day == mappedSelectedDayWithRef.Day);

                if (dbRoute == null)
                {
                    return NotFound();
                }

                List<string> coordinatesList = [];

                if (!string.IsNullOrWhiteSpace(mappedSelectedDayWithRef.GeoLocation))
                {
                    coordinatesList.Add(mappedSelectedDayWithRef.GeoLocation);
                }

                foreach (Customer dbCustomer in dbRoute.Customers.OrderBy(p => p.Position).ToList() ?? [])
                {
                    MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == mappedSelectedDayWithRef.Year && x.Month == mappedSelectedDayWithRef.Month);
                    bool toDeliver = false;

                    if (dbFoundOverview != null)
                    {
                        int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == mappedSelectedDayWithRef.Day)).NumberOfBoxes;
                        toDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, mappedSelectedDayWithRef.Year, mappedSelectedDayWithRef.Month, mappedSelectedDayWithRef.Day));
                    }
                    else //Safety Measure
                    {
                        toDeliver = CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, mappedSelectedDayWithRef.Year, mappedSelectedDayWithRef.Month, mappedSelectedDayWithRef.Day);
                    }

                    if (toDeliver)
                    {
                        coordinatesList.Add($"{dbCustomer.DeliveryLocation.Latitude},{dbCustomer.DeliveryLocation.Longitude}");
                    }
                }

                if ((string.IsNullOrWhiteSpace(mappedSelectedDayWithRef.GeoLocation) && coordinatesList.Count == 0) || (!string.IsNullOrWhiteSpace(mappedSelectedDayWithRef.GeoLocation) && coordinatesList.Count == 1))
                {
                    return NoContent();
                }

                const int maxWaypoints = 150; // TomTom limit
                List<DTOPoints> allPoints = [];

                for (int i = 0; i < coordinatesList.Count; i += (maxWaypoints - 1)) // Overlap by 1 waypoint
                {
                    var chunk = coordinatesList.Skip(i).Take(maxWaypoints).ToList();
  
                    var response = await httpClient.GetFromJsonAsync<DTORouteResponse>($"https://api.tomtom.com/routing/1/calculateRoute/{string.Join(":", chunk)}/json?key={Environment.GetEnvironmentVariable("ROUTING_API")}");

                    if (response?.Routes != null)
                    {
                        var chunkPoints = response.Routes
                                                  .SelectMany(route => route.Legs)
                                                  .SelectMany(leg => leg.Points)
                                                  .Select(point => new DTOPoints
                                                  {
                                                      Latitude = point.Latitude,
                                                      Longitude = point.Longitude
                                                  })
                                                  .ToList();

                        if (allPoints.Count > 0 && chunkPoints.Count > 0)
                        {
                            chunkPoints.RemoveAt(0);
                        }

                        allPoints.AddRange(chunkPoints);
                    }

                    if (chunk.Count < maxWaypoints)
                    {
                        break;
                    }
                }

                if (allPoints.Count > 0)
                {
                    return Ok(allPoints);
                }
                else
                {
                    return NoContent();
                }
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

        [HttpPost("GetCustomerToDeliver", Name = "GetCustomerToDeliver")]
        public async Task<IActionResult> GetCustomerToDeliver([FromBody] DTOSelectedDayWithReference dTOSelectedDayWithReference)
        {
            try
            {
                var mappedSelectedDayWithRef = mapper.Map<MappedSelectedDayWithReference>(dTOSelectedDayWithReference);

                Route? dbRoute = await context.Routes.AsNoTracking().Where(r => r.Id != long.MinValue && r.Id == mappedSelectedDayWithRef.ReferenceId)
                                                      .Include(r => r.Customers).ThenInclude(c => c.DeliveryLocation)
                                                      .Include(r => r.Customers).ThenInclude(c => c.Workdays)
                                                      .Include(r => r.Customers).ThenInclude(c => c.Holidays)
                                                      .Include(r => r.Customers).ThenInclude(m => m.MonthlyOverviews.Where(x => x.Year == mappedSelectedDayWithRef.Year && x.Month == mappedSelectedDayWithRef.Month))
                                                                                .ThenInclude(d => d.DailyOverviews.Where(x => x.DayOfMonth == mappedSelectedDayWithRef.Day))
                                                      .FirstOrDefaultAsync();
                Holiday? holiday = await context.Holidays.AsNoTracking().FirstOrDefaultAsync(h => h.Country.Equals(currentCountry) && h.Year == mappedSelectedDayWithRef.Year && h.Month == mappedSelectedDayWithRef.Month && h.Day == mappedSelectedDayWithRef.Day);

                if (dbRoute == null)
                {
                    return NotFound();
                }

                List<Customer> customers = [];
                foreach (Customer dbCustomer in dbRoute.Customers.OrderBy(p => p.Position).ToList() ?? [])
                {
                    MonthlyOverview? dbFoundOverview = dbCustomer.MonthlyOverviews.FirstOrDefault(x => x.Year == mappedSelectedDayWithRef.Year && x.Month == mappedSelectedDayWithRef.Month);
                    bool toDeliver = false;

                    if (dbFoundOverview != null)
                    {
                        int? numberOfBoxes = ((DailyOverview)dbFoundOverview.DailyOverviews.First(x => x.DayOfMonth == mappedSelectedDayWithRef.Day)).NumberOfBoxes;
                        toDeliver = numberOfBoxes > 0 || (numberOfBoxes == null && CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, mappedSelectedDayWithRef.Year, mappedSelectedDayWithRef.Month, mappedSelectedDayWithRef.Day));
                    }
                    else //Safety Measure
                    {
                        toDeliver = CheckMonthlyOverview.GetDeliveryTrueOrFalse(dbCustomer, holiday, mappedSelectedDayWithRef.Year, mappedSelectedDayWithRef.Month, mappedSelectedDayWithRef.Day);
                    }

                    if (toDeliver)
                    {
                        customers.Add(dbCustomer);
                    }
                }
              
                return Ok(mapper.Map<List<DTOCustomersLocation>>(customers));
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
    }
}
