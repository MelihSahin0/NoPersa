using AutoMapper;
using DeliveryService.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
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
                for (int i = 0; i < routes.Routes?.Length; i++)
                {
                    Route route = mapper.Map<Route>(routes.Routes[i]);
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

        [HttpGet("GetRoutesDetails", Name = "GetRoutesDetails")]
        public IActionResult GetRoutesDetails()
        {
            try
            {
                DTORoutes dTORoutes = new();
                List<DTORoute> routes = [];

                foreach (Route route in context.Routes.Include(r => r.Customers))
                {
                    routes.Add(mapper.Map<DTORoute>(route));
                }
                dTORoutes.Routes = [.. routes];

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

        [HttpGet("GetRoutes", Name = "GetRoutes")]
        public IActionResult GetRoutes()
        {
            try
            {
                DTORoutes dTORoutes = new();
                List<DTORoute> routes = [];

                foreach (Route route in context.Routes)
                {
                    routes.Add(mapper.Map<DTORoute>(route));
                }
                dTORoutes.Routes = [.. routes];

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
    }
}
