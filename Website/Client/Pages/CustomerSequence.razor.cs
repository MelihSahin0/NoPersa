using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.Exceptions;
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

        public required CustomerSeq CustomerSeq { get; set; }

        protected override void OnInitialized()
        {
            CustomerSeq = new CustomerSeq() {  RouteDetails = [], SelectedRouteDetailsId = [], RouteOverview = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetRouteDetails")!;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                CustomerSeq.RouteDetails = [.. JsonSerializer.Deserialize<List<SequenceDetails>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.OrderBy(r => r.Name)];
                CustomerSeq.SelectedRouteDetailsId = CustomerSeq.RouteDetails.Take(2).Select(r => r.Id).ToArray();
                CustomerSeq.RouteOverview = [.. CustomerSeq.RouteDetails.Select(r => new RouteOverview() { Id = r.Id, Name = r.Name, Position = 0, NumberOfCustomers = 0}).OrderBy(r => r.Name)];
            }
            else
            {
                throw new ServiceNotReachableException();
            }
        }

        private async Task Submit(EditContext editContext)
        {

            using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/UpdateCustomerSequence", CustomerSeq.RouteDetails, JsonSerializerOptions);

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

        private void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
