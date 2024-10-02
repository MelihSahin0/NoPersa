using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.Exceptions;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using Website.Client.FormModels;

namespace Website.Client.Pages
{
    public partial class ModifyRoutes
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

        public required Route Routes { get; set; }

        protected override void OnInitialized()
        {
            Routes = new Route() { Routes = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagment/GetRoutes")!;
           
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Routes = JsonSerializer.Deserialize<Route>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
            }
            else
            {
                throw new Exception();
            }
        }

        private async Task Submit(EditContext editContext)
        {
            using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagment/AddRoutes", Routes, JsonSerializerOptions);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NotificationService.SetSuccess("Successfully added/updated route(s)");
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
