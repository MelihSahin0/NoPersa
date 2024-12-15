using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.Services;
using Website.Client.FormModels;
using Website.Client.Models;
using System.Net.Http.Json;
using Website.Client.Components.Default;

namespace Website.Client.Pages
{
    public partial class CustomersInRoute
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

        public bool IsSubmitting { get; set; } = false;
        public required CustomerSequenceModel CustomerSequenceModel { get; set; }

        protected override void OnInitialized()
        {
            CustomerSequenceModel = new CustomerSequenceModel() {  SequenceDetails = [], SelectedRouteDetailsId = [], RouteOverview = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try 
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/DeliveryManagement/GetRouteDetails")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CustomerSequenceModel.SequenceDetails = [.. JsonSerializer.Deserialize<List<SequenceDetail>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.OrderBy(r => r.Name)];
                    CustomerSequenceModel.SelectedRouteDetailsId = CustomerSequenceModel.SequenceDetails.Take(2).Select(r => r.Id).ToArray();
                    CustomerSequenceModel.RouteOverview = [.. CustomerSequenceModel.SequenceDetails.Select(r => new SelectInput() { Id = r.Id, Value = r.Name}).OrderBy(r => r.Value)];
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }

        private async Task Submit(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/DeliveryManagement/UpdateCustomerSequence", CustomerSequenceModel.SequenceDetails, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated customer sequence");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
            IsSubmitting = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
