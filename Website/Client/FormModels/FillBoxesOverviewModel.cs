using Blazored.LocalStorage;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;
using Website.Client.Models;
using Website.Client.Services;
using Website.Client.Enums;
using System.Net.Http.Json;

namespace Website.Client.FormModels
{
    public class FillBoxesOverviewModel
    {
        [JsonIgnore]
        public ILocalStorageService LocalStorage { get; init; }

        [JsonIgnore]
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        [JsonIgnore]
        public HttpClient HttpClient { get; init; }

        [JsonIgnore]
        public NotificationService NotificationService { get; init; }

        public FillBoxesOverviewModel(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService)
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
        }

        [ValidateComplexType]
        [Required]
        public required List<FoodOverview> FoodOverview { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<RoutesFoodOverview> RoutesFoodOverview { get; set; }

        [Required]
        public required int DisplayTypeIndex { get; set; }

        [Required]
        public required int FoodOverviewIndex { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month { get; set; }

        [Required]
        public required int Day { get; set; }

        public HashSet<RoutesFoodOverview> ExpandedRoutes { get; set; } = [];
        public void ToggleRoute(RoutesFoodOverview route)
        {
            if (!ExpandedRoutes.Remove(route))
            {
                ExpandedRoutes.Add(route);
            }
        }

        public HashSet<CustomersFood> ClickedCustomer { get; set; } = [];
        public CustomersFood? DisplayCustomer { get; set; }
        public void SetDisplayCustomer(CustomersFood customer)
        {
            DisplayCustomer = customer;
            ClickedCustomer.Add(customer);
        }

        public async Task OnDayMonthYearSelected()
        {
            try
            {
                if (DisplayTypeIndex == 0)
                {
                    using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/GetFoodOverview",
                        new
                        {
                            Year = Year,
                            Month = Month,
                            Day = Day,
                        }, JsonSerializerOptions)!;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        FoodOverview = [.. JsonSerializer.Deserialize<List<FoodOverview>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!];
                    }
                    else
                    {
                        NotificationService.SetError(await response.Content.ReadAsStringAsync());
                    }
                }
                else
                {
                    using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/GetRoutesFoodOverview",
                                           new
                                           {
                                               Year = Year,
                                               Month = Month,
                                               Day = Day,
                                           }, JsonSerializerOptions)!;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        RoutesFoodOverview = [.. JsonSerializer.Deserialize<List<RoutesFoodOverview>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!];

                        RoutesFoodOverview? routesFoodOverview = RoutesFoodOverview.FirstOrDefault(r => r.Position == 0);
                        if (routesFoodOverview != null)
                        {
                            ToggleRoute(routesFoodOverview);
                        }
                    }
                    else
                    {
                        NotificationService.SetError(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }
    }
}