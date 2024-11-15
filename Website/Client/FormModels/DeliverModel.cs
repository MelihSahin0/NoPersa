using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Website.Client.Components.Default;
using Website.Client.Enums;
using Website.Client.Models;
using Website.Client.Services;

namespace Website.Client.FormModels
{
    public class DeliverModel
    {
        [JsonIgnore]
        public ILocalStorageService LocalStorage { get; init; }

        [JsonIgnore]
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        [JsonIgnore]
        public HttpClient HttpClient { get; init; }

        [JsonIgnore]
        public NotificationService NotificationService { get; init; }

        [JsonIgnore]
        public GeoLocationService GeoLocationService { get; set; }

        [JsonIgnore]
        public LeafletService LeafletService { get; set; }

        public DeliverModel(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService, GeoLocationService geoLocationService, LeafletService leafletService) 
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
            GeoLocationService = geoLocationService;
            LeafletService = leafletService;
        }

        [Required]
        [IntType]
        public required int RouteId { get; set; }

        [JsonIgnore]
        public List<SelectInput>? RouteSummary { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month { get; set; }

        [Required]
        public required int Day { get; set; }

        [Required]
        public required List<DeliverSummary> CustomerDelivery { get; set; }

        public int CustomerIndex = 0;

        [JsonIgnore]
        public bool IsSubmitting { get; set; } = false;

        public async Task GetRouteCustomers(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetCustomerToDeliver", new
                {
                    ReferenceId = RouteId,
                    Year = Year,
                    Month = Month,
                    Day = Day,
                }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CustomerDelivery = [.. JsonSerializer.Deserialize<List<DeliverSummary>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!];

                    if (CustomerDelivery.Count != 0)
                    {
                        displayCustomer = CustomerDelivery[0];
                    }
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

        public async Task ShowMap()
        {
            try
            {
                Location? location = await GeoLocationService.GetCurrentLocation();

                if (!LeafletService.CoordinatesSet())
                {
                    using var response2 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetRouting", new
                    {
                        ReferenceId = RouteId,
                        Year = Year,
                        Month = Month,
                        Day = Day,
                        GeoLocation = location != null ? $"{location.Latitude.ToString().Replace(",", ".")},{location.Longitude.ToString().Replace(",", ".")}" : ""
                    }, JsonSerializerOptions)!;

                    if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var route = JsonSerializer.Deserialize<List<Location>>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!;

                        if (route == null || route.Count == 0)
                        {
                            NotificationService.SetError("No route to calculate");
                        }
                        else
                        {
                            LeafletService.SetCoordinates(route.Select(r => new[] { r.Latitude, r.Longitude }).ToArray());

                            if (location != null)
                            {
                                await LeafletService.Init(location.Latitude, location.Longitude, 14, await LocalStorage.GetItemAsync<string>("DeliveryService"));
                                await HandleLocationUpdated(location);
                            }
                            else
                            {
                                await LeafletService.Init(route[0].Latitude, route[0].Longitude, 14, await LocalStorage.GetItemAsync<string>("DeliveryService"));
                            }

                            await LeafletService.DrawAllRoute();
                            await HandleMarker();

                            GeoLocationService.OnLocationUpdated += HandleLocationUpdated;
                        }
                    }
                    else
                    {
                        NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                    }
                }
            } 
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }

        private async Task HandleMarker()
        {
            await LeafletService.ClearMarkers();

            List<LeafletService.Marker> markers = [];
            int imageId = 0;
            for (int i = 0; i < CustomerDelivery.Count; i++)
            {
                if (CustomerIndex == i)
                {
                    imageId++;
                }
                if (CustomerIndex < i)
                {
                    imageId = 2;
                }

                markers.Add(new LeafletService.Marker()
                {
                    Latitude = CustomerDelivery[i].Latitude,
                    Longitude = CustomerDelivery[i].Longitude,
                    PopupText = CustomerDelivery[i].Name,
                    ImageId = imageId
                });
            }

            await LeafletService.AddMarker(markers);
        }

        private async Task HandleLocationUpdated(Location location)
        {
            Console.WriteLine($"{location.Latitude}, {location.Longitude}");
            await LeafletService.ClearMyMarker();
            await LeafletService.AddMyMarker(new LeafletService.Marker() { Latitude = location.Latitude, Longitude = location.Longitude, PopupText = "Me" });
        }

        public async Task Submit(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {

            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
            IsSubmitting = false;
        }

        public bool ButtonPrevious { get; set; } = false;

        public bool ButtonNext { get; set; } = true;

        public bool ButtonSubmit { get; set; } = false;

        public DeliverSummary? displayCustomer;
        public async Task Next()
        {
            CustomerIndex++;
            if (CustomerIndex < CustomerDelivery.Count)
            {
                displayCustomer = CustomerDelivery[CustomerIndex];
                CheckButtons();
                await HandleMarker();
            }
        }

        public async Task Previous()
        {
            CustomerIndex--;
            if (CustomerIndex >= 0)
            {
                displayCustomer = CustomerDelivery[CustomerIndex];
                CheckButtons();
                await HandleMarker();
            }
        }

        private void CheckButtons()
        {
            if (CustomerIndex == 0)
            {
                ButtonPrevious = false;
                ButtonNext = true;
                ButtonSubmit = false;
            }
            else if (CustomerIndex == CustomerDelivery.Count - 1)
            {
                ButtonPrevious = true;
                ButtonNext = false;
                ButtonSubmit = true;
            }
            else 
            {
                ButtonPrevious = true;
                ButtonNext = true;
                ButtonSubmit = false;
            }
        }
    }
}
