using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Exceptions;
using Website.Client.FormModels;
using Website.Client.Models;

namespace Website.Client.Pages
{
    public partial class DeliveryStatus
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

        public required DeliveryStats DeliveryStats { get; set; }

        protected override void OnInitialized()
        {
            DeliveryStats = new DeliveryStats()
            {
                RouteDetails = []
            };
            TabIndex = 0;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagment/GetRoutesDetails",
                    new {
                            Year = "2024",
                            Month = 9,
                            Day = 7,
                        }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DeliveryStats.RouteDetails = [.. JsonSerializer.Deserialize<DeliveryStats>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.RouteDetails.OrderBy(x => x.Position)];
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

        public required int TabIndex { get; set; }
    }
}
