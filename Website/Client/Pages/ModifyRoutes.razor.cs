using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using Website.Client.FormModels;
using Website.Client.Services;
using Website.Client.Models;

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

        public bool IsSubmitting { get; set; } = false;

        public required ModifyRoutesModel ModifyRoutesModel { get; set; }

        protected override void OnInitialized()
        {
            ModifyRoutesModel = new ModifyRoutesModel() { RouteDeliverableSummary = [], RouteNonDeliverableSummary = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/DeliveryManagement/GetRoutesSummary")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var routes = JsonSerializer.Deserialize<List<RouteSummary>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                    ModifyRoutesModel.RouteDeliverableSummary = [.. routes.Where(r => r.IsDrivable)];
                    ModifyRoutesModel.RouteNonDeliverableSummary = [.. routes.Where(r => !r.IsDrivable)];
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
                List<RouteSummary> routes = [];
                routes.AddRange(ModifyRoutesModel.RouteDeliverableSummary);
                routes.AddRange(ModifyRoutesModel.RouteNonDeliverableSummary);
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/DeliveryManagement/UpdateRoutes", routes, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated routes");
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
