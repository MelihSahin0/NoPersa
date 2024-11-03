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
    public class FillBoxes
    {
        [JsonIgnore]
        public ILocalStorageService LocalStorage { get; init; }

        [JsonIgnore]
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        [JsonIgnore]
        public HttpClient HttpClient { get; init; }

        [JsonIgnore]
        public NotificationService NotificationService { get; init; }

        public FillBoxes(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService)
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
        }

        [ValidateComplexType]
        [Required]
        public required List<FoodOverview> FoodOverview { get; set; }

        //missing Routes

        [Required]
        public required int DisplayTypeIndex { get; set; }

        [Required]
        public required int FoodOverviewIndex { get; set; }

        [Required]
        public required int RouteOverviewIndex { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month { get; set; }

        [Required]
        public required int Day { get; set; }

        public async Task OnDayMonthYearSelected()
        {
            try
            {
                if (DisplayTypeIndex == 0)
                {
                    using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/GetFoodOverview",
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
                    //TODO
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }
    }
}
