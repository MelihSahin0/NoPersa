using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Components.Default;
using Website.Client.Enums;
using Website.Client.Models;
using Website.Client.Services;
using Website.Client.Util;

namespace Website.Client.FormModels
{
    public class DeliverModel : IDisposable
    {
        public ILocalStorageService LocalStorage { get; init; }

        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        public HttpClient HttpClient { get; init; }

        public NotificationService NotificationService { get; init; }

        public NavigationManager NavigationManager { get; set; }

        public GeoLocationService GeoLocationService { get; set; }

        public LeafletService LeafletService { get; set; }

        public DeliverModel(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService, NavigationManager navigationManager, GeoLocationService geoLocationService, LeafletService leafletService) 
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
            NavigationManager = navigationManager;
            GeoLocationService = geoLocationService;
            LeafletService = leafletService;
        }

        [Required]
        [IntType]
        public required int RouteId { get; set; }

        public List<SelectInput>? RouteSummary { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month { get; set; }

        [Required]
        public required int Day { get; set; }

        [Required]
        public required List<DeliverSummary> CustomerDelivery { get; set; }

        public int CustomerIndex { get; set; } = 0;

        public bool IsSubmitting { get; set; } = false;

        public bool DisplayMap { get; set; } = false;

        public async Task GetRouteCustomers(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetCustomerToDeliver", new
                {
                    ReferenceId = RouteId,
                    Year,
                    Month,
                    Day,
                }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CustomerDelivery = [.. JsonSerializer.Deserialize<List<DeliverSummary>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!];
                    CustomersBoxStatuses = [.. CustomerDelivery.Select(r => new CustomersBoxStatus() { Id = r.Id, DeliveredBoxes = r.NumberOfBoxes, ReceivedBoxes = r.NumberOfBoxes })];

                    if (CustomerDelivery.Count != 0)
                    {
                        displayCustomer = CustomerDelivery[0];

                        if (CustomerDelivery.Count == 1)
                        {
                            ButtonPrevious = false;
                            ButtonNext = false;
                            ButtonSubmit = true;
                        }
                        else
                        {
                            ButtonPrevious = false;
                            ButtonNext = true;
                            ButtonSubmit = false;
                        }
                    }
                    else
                    {
                        NotificationService.SetError("No customers today");
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
            if (DisplayMap)
            {
                try
                {
                    Location? location = await GeoLocationService.GetCurrentLocation();

                    if (!LeafletService.CoordinatesSet())
                    {
                        using var response2 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetRouting", new
                        {
                            ReferenceId = RouteId,
                            Year,
                            Month,
                            Day,
                            GeoLocation = location != null ? $"{location.Latitude.ToString().Replace(",", ".")},{location.Longitude.ToString().Replace(",", ".")}" : ""
                        }, JsonSerializerOptions)!;

                        if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var route = JsonSerializer.Deserialize<List<Location>>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!;

                            if (route == null || route.Count == 0)
                            {
                                DisplayMap = false;
                                NotificationService.SetError("No route to calculate");
                            }
                            else
                            {
                                LeafletService.SetCoordinates(route.Select(r => new[] { r.Latitude, r.Longitude }).ToArray());

                                await LeafletService.Init(14, await LocalStorage.GetItemAsync<string>("DeliveryService"));

                                if (location != null)
                                {
                                    await HandleLocationUpdated(location);
                                }

                                await LeafletService.DrawAllRoute();
                                await HandleMarker();

                                GeoLocationService.OnLocationUpdated += HandleLocationUpdated;
                            }
                        }
                        else
                        {
                            DisplayMap = false;
                            NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                        }
                    }
                    else
                    {
                        await LeafletService.Init(14, await LocalStorage.GetItemAsync<string>("DeliveryService"));

                        if (location != null)
                        {   
                            await HandleLocationUpdated(location);
                        }

                        await LeafletService.DrawAllRoute();
                        await HandleMarker();

                        GeoLocationService.OnLocationUpdated += HandleLocationUpdated;
                    }
                }
                catch
                {
                    DisplayMap = false;
                    NotificationService.SetError("Server is not reachable.");
                }
            }
            else
            {
                await LeafletService.ClearMap();
                GeoLocationService.OnLocationUpdated -= HandleLocationUpdated;
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
            await LeafletService.ClearMyMarker();
            await LeafletService.AddMyMarker(new LeafletService.Marker() { Latitude = location.Latitude, Longitude = location.Longitude, PopupText = "Me" });
        }

        public async Task Submit(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                await LeafletService.ClearMap();
                GeoLocationService.OnLocationUpdated -= HandleLocationUpdated;
                LeafletService.SetCoordinates([]);

                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/BoxManagement/UpdateCustomersBoxStatus", CustomersBoxStatuses, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated customers box status");
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

        public List<SelectInput> DefaultNumbers = Misc.GetDefaultNumberOfBoxesSelection;

        public bool ButtonPrevious { get; set; } = false;

        public bool ButtonNext { get; set; } = false;

        public bool ButtonSubmit { get; set; } = false;

        [Required]
        public required List<CustomersBoxStatus> CustomersBoxStatuses {get; set;}

        public DeliverSummary? displayCustomer;
        public async Task Next()
        {
            CustomerIndex++;
            if (CustomerIndex < CustomerDelivery.Count)
            {
                displayCustomer = CustomerDelivery[CustomerIndex];
            }
            CheckButtons();
            await HandleMarker();
        }

        public async Task Previous()
        {
            CustomerIndex--;
            if (CustomerIndex >= 0)
            {
                displayCustomer = CustomerDelivery[CustomerIndex];
            }
            CheckButtons();
            await HandleMarker();
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

        public void Dispose()
        {
            GeoLocationService.OnLocationUpdated -= HandleLocationUpdated;
        }
    }
}
