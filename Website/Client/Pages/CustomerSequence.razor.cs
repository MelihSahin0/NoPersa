using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.Services;
using Website.Client.FormModels;
using Website.Client.Models;
using System.Net.Http.Json;

namespace Website.Client.Pages
{
    public partial class CustomerSequence
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
        public required CustomerSeq CustomerSeq { get; set; }

        protected override void OnInitialized()
        {
            CustomerSeq = new CustomerSeq() {  SequenceDetails = [], SelectedRouteDetailsId = [], RouteOverview = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try 
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetRouteDetails")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CustomerSeq.SequenceDetails = [.. JsonSerializer.Deserialize<List<SequenceDetail>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.OrderBy(r => r.Name)];
                    CustomerSeq.SelectedRouteDetailsId = CustomerSeq.SequenceDetails.Take(2).Select(r => r.Id).ToArray();
                    CustomerSeq.RouteOverview = [.. CustomerSeq.SequenceDetails.Select(r => new RouteOverview() { Id = r.Id, Name = r.Name, Position = 0, NumberOfCustomers = 0 }).OrderBy(r => r.Name)];
                }
                else
                {
                    NotificationService.SetError(await response.Content.ReadAsStringAsync());
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
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/UpdateCustomerSequence", CustomerSeq.SequenceDetails, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated customer sequence");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    NotificationService.SetError(await response.Content.ReadAsStringAsync());
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
