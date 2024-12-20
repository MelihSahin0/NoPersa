using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OpenXml;
using OpenXml.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Enums;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class InvoiceExport
    {
        [Inject]
        public required ILocalStorageService LocalStorage { get; set; }

        [Inject]
        public required JsonSerializerOptions JsonSerializerOptions { get; set; }

        [Inject]
        public required HttpClient HttpClient { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [Inject]
        public required NotificationService NotificationService { get; set; }

        [Inject]
        public required UtilityService UtilityService { get; set; }

        public required InvoiceExportModel InvoiceExportModel { get; set; }

        public bool IsSubmitting { get; set; } = false;

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Today;
            InvoiceExportModel = new InvoiceExportModel()
            {
                SheetNames = [],
                Header = [],
                Invoices = [],
                SplitToMultipleRoutes = false,
                ShowAllDays = false,
                Year = dateTime.Year,
                Month = (Months)dateTime.Month,
            };
        }

        public async Task GetExcelInvoice()
        {
            IsSubmitting = true;
            InvoiceExportModel.ExcelFileBytes = [];
            sheetNumber = 0;

            try
            {
                using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/ImportExportManagement/CreateInvoice", new
                {
                    InvoiceExportModel.Year,
                    InvoiceExportModel.Month,
                    InvoiceExportModel.SplitToMultipleRoutes,
                    InvoiceExportModel.ShowAllDays,
                }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    InvoiceExportModel.ExcelFileBytes = await response.Content.ReadAsByteArrayAsync();
                    var table = SpreadSheet.ReadSpreadSheet(InvoiceExportModel.ExcelFileBytes, InvoiceExportModel.SplitToMultipleRoutes, 10);
                    InvoiceExportModel.SheetNames = [.. table.Select(t => t.Name)];
                    InvoiceExportModel.Header = [.. table.First().Headers];
                    InvoiceExportModel.Invoices = Invoice.Parse(table);
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                }
            }
            catch (Exception e)
            {
                NotificationService.SetError(e.Message);
            }

            IsSubmitting = false;
        }

        public int sheetNumber;
        private void ChangeSheet(int change)
        {
            sheetNumber += change;
        }

        public void Download()
        {
            if (InvoiceExportModel.ExcelFileBytes != null)
            {
                UtilityService.DownLoadFile("report.xlsx", InvoiceExportModel.ExcelFileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
    }
}
