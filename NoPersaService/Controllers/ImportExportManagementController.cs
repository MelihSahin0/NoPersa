using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoPersaService.Database;
using NoPersaService.DTOs.ImportExport.Receive;
using NoPersaService.Models;
using OpenXml.Models;
using OpenXml;

namespace NoPersaService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportExportManagementController : ControllerBase
    {
        private readonly NoPersaDbContext context;
        private readonly ILogger<ImportExportManagementController> logger;
        private readonly IMapper mapper;

        public ImportExportManagementController(ILogger<ImportExportManagementController> logger, NoPersaDbContext noPersaDbContext, IMapper mapper)
        {
            this.context = noPersaDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpPost("CreateInvoice", Name = "CreateInvoice")]
        public IActionResult CreateInvoice([FromBody] DTOInvoice dTOInvoice)
        {
            try
            {
                List<string> header = Invoice.InvoiceHeader;
                if (dTOInvoice.ShowAllDays)
                {
                    int daysInMonth = DateTime.DaysInMonth(dTOInvoice.Year, dTOInvoice.Month);
                    header.AddRange(Enumerable.Range(1, daysInMonth).Select(day => $"Day {day}"));
                }

                var monthlyOverviews = mapper.Map<List<Invoice>>(context.MonthlyOverviews.AsNoTracking().Where(m => m.Year == dTOInvoice.Year && m.Month == dTOInvoice.Month)
                                            .Include(m => m.DailyOverviews)
                                            .Include(m => m.Customer).ThenInclude(c => c!.Route)
                                            .ToList());

                var spreadsheetBytes = SpreadSheet.CreateSpreadsheet(Invoice.Parse(monthlyOverviews, header, dTOInvoice.SplitToMultipleRoutes, dTOInvoice.ShowAllDays));

                return File(spreadsheetBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"report.xlsx");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed creating invoice");
                return BadRequest("An error occurred while processing your request.");
            }
        }

        [HttpPost("ExportCustomers", Name = "ExportCustomers")]
        public IActionResult ExportCustomers([FromBody] DTOExportCustomer dTOExportCustomer)
        {
            try
            {
                List<string> header = ExcelCustomer.ExcelCustomerHeader;
                var customer = mapper.Map<List<ExcelCustomer>>(context.Customers.AsNoTracking().Include(r => r.Route)
                                                               .Include(dl => dl.DeliveryLocation)
                                                               .Include(a => a.Article)
                                                               .Include(w => w.Workdays).Include(h => h.Holidays)
                                                               .Include(cmp => cmp.CustomerMenuPlans).ThenInclude(b => b.BoxContent)
                                                               .Include(cmp => cmp.CustomerMenuPlans).ThenInclude(p => p.PortionSize));

                List<string> routes = [.. context.Routes.AsNoTracking().OrderBy(r => r.Name).Select(r => r.Name)];
                List<string> articles = [.. context.Articles.AsNoTracking().OrderBy(a => a.Position).Select(a => a.Name)];
                List<string> portionSize = [.. context.PortionSizes.AsNoTracking().OrderBy(p => p.Position).Select(a => a.Name)];
                List<string> trueAndFalse = ["FALSE", "TRUE"];
                header.AddRange(context.BoxContents.AsNoTracking().Select(b => b.Name));

                var spreadsheetBytes = SpreadSheet.CreateSpreadsheet(ExcelCustomer.Parse(customer, header, routes, articles, portionSize, trueAndFalse, dTOExportCustomer.SplitToMultipleRoutes));
                return File(spreadsheetBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"customers.xlsx");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed exporting customer");
                return BadRequest("An error occurred while processing your request.");
            }
        }
    }
}
