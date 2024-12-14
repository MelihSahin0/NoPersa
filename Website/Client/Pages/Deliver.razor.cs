using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using Website.Client.Components.Default;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class Deliver
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
        public required GeoLocationService GeoLocationService { get; set; }

        [Inject]
        public required LeafletService LeafletService { get; set; }

        public required DeliverModel DeliverModel { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Today;
            DeliverModel = new DeliverModel(LocalStorage, JsonSerializerOptions, HttpClient, NotificationService, NavigationManager, GeoLocationService, LeafletService)
            {
                RouteId = -1,
                RouteSummary = [],
                Year = dateTime.Year,
                Month = (Enums.Months)(dateTime.Month),
                Day = dateTime.Day,
                CustomerDelivery = [],
                CustomersBoxStatuses = [],
            };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/DeliveryManagement/GetRoutesSummary")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DeliverModel.RouteSummary = JsonSerializer.Deserialize<List<RouteSummary>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.Select(r => new SelectInput() { Id = r.Id, Value = r.Name }).ToList();
                    DeliverModel.RouteId = (DeliverModel.RouteSummary.FirstOrDefault()?.Id ?? -1);
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
    }
}

