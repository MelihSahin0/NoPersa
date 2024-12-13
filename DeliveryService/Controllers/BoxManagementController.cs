using AutoMapper;
using DeliveryService.Database;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DTOs.Delivery;
using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoxManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<BoxManagementController> logger;
        private readonly IMapper mapper;
        private readonly HttpClient httpClient;

        public BoxManagementController(ILogger<BoxManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper, HttpClient httpClient)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
            this.httpClient = httpClient;
        }

        [HttpGet("GetBoxStatus", Name = "GetBoxStatus")]
        public IActionResult GetBoxStatus()
        {
            try
            {
                List<DTOBoxStatus> dTOBoxStatuses = mapper.Map<List<DTOBoxStatus>>(context.BoxStatuses.AsNoTracking().Include(b => b.Customer).ThenInclude(c => c.Route));

                return Ok(dTOBoxStatuses.OrderBy(x => (x.RouteName, x.CustomersName)));
            }
            catch (ValidationException e)
            {
                logger.LogError(e, "Could not map box status");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Getting box status failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateBoxStatus", Name = "UpdateBoxStatus")]
        public IActionResult UpdateBoxStatus(List<DTOBoxStatus> dTOBoxStatuses)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                List<BoxStatus> boxStatuses = mapper.Map<List<BoxStatus>>(dTOBoxStatuses);

                var existingBoxStatuses = context.BoxStatuses.ToDictionary(a => a.Id);
                foreach (var boxStatus in boxStatuses)
                {
                    if (existingBoxStatuses.TryGetValue(boxStatus.Id, out var foundBoxStatus))
                    {
                        foundBoxStatus.NumberOfBoxesPreviousDay = boxStatus.NumberOfBoxesPreviousDay;
                        foundBoxStatus.DeliveredBoxes = boxStatus.DeliveredBoxes;
                        foundBoxStatus.ReceivedBoxes = boxStatus.ReceivedBoxes;
                        foundBoxStatus.NumberOfBoxesCurrentDay = boxStatus.NumberOfBoxesCurrentDay;
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map box status");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating box status failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("UpdateCustomersBoxStatus", Name = "UpdateCustomersBoxStatus")]
        public IActionResult UpdateUpdateCustomersBoxStatusRoutes([FromBody] List<DTOCustomersBoxStatus> dTOCustomersBoxStatuses)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                var customerIds = dTOCustomersBoxStatuses.Select(status => status.Id).ToList();
                var dbCustomers = context.Customers.Where(c => customerIds.Contains(c.Id)).ToList();
                var dbBoxStatuses = context.BoxStatuses.Where(b => customerIds.Contains(b.CustomerId)).ToList();

                foreach (var customersBoxStatus in dTOCustomersBoxStatuses)
                {
                    var dbBoxStatus = dbBoxStatuses.FirstOrDefault(b => b.CustomerId == customersBoxStatus.Id);

                    if (dbBoxStatus == null)
                    {
                        var customer = dbCustomers.FirstOrDefault(c => c.Id == customersBoxStatus.Id);
                        if (customer == null)
                        {
                            logger.LogWarning($"Customer with ID {customersBoxStatus.Id} not found.");
                            continue;
                        }

                        context.BoxStatuses.Add(new BoxStatus
                        {
                            CustomerId = customer.Id,
                            Customer = customer,
                            NumberOfBoxesPreviousDay = 0,
                            DeliveredBoxes = customersBoxStatus.DeliveredBoxes,
                            ReceivedBoxes = customersBoxStatus.ReceivedBoxes,
                            NumberOfBoxesCurrentDay = customersBoxStatus.DeliveredBoxes - customersBoxStatus.ReceivedBoxes
                        });
                    }
                    else
                    {
                        dbBoxStatus.DeliveredBoxes = customersBoxStatus.DeliveredBoxes;
                        dbBoxStatus.ReceivedBoxes = customersBoxStatus.ReceivedBoxes;
                        dbBoxStatus.NumberOfBoxesCurrentDay += customersBoxStatus.DeliveredBoxes - customersBoxStatus.ReceivedBoxes;
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (ValidationException e)
            {
                transaction.Rollback();
                logger.LogError(e, "Could not map customers box status");
                return ValidationProblem("The request contains invalid data: " + e.Message);
            }
            catch (Exception e)
            {
                transaction.Rollback();
                logger.LogError(e, "Updating customers box status failed");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
