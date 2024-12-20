using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OpenXml;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;
using OpenXml.Models;

namespace Website.Client.Pages
{
    public partial class CustomerExport
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

        public required CustomerExportModel CustomerExportModel { get; set; }

        public bool IsSubmitting { get; set; } = false;

        protected override void OnInitialized()
        {
            CustomerExportModel = new CustomerExportModel()
            {
                SheetNames = [],
                Header = [],
                Customers = [],
                SplitToMultipleRoutes = false
            };
        }

        public async Task GetExcelInvoice()
        {
            IsSubmitting = true;
            CustomerExportModel.ExcelFileBytes = [];
            sheetNumber = 0;

            try
            {
                using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/ImportExportManagement/ExportCustomers", new
                {
                    CustomerExportModel.SplitToMultipleRoutes
                }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CustomerExportModel.ExcelFileBytes = await response.Content.ReadAsByteArrayAsync();
                    var table = SpreadSheet.ReadSpreadSheet(CustomerExportModel.ExcelFileBytes, CustomerExportModel.SplitToMultipleRoutes, 10);
                    CustomerExportModel.SheetNames = [.. table.Select(t => t.Name)];
                    CustomerExportModel.Header = [.. table.First().Headers];
                    CustomerExportModel.Customers = ExcelCustomer.Parse(table);
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
            if (CustomerExportModel.ExcelFileBytes != null)
            {
                UtilityService.DownLoadFile("customer.xlsx", CustomerExportModel.ExcelFileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }
    }
}
