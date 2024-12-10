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
