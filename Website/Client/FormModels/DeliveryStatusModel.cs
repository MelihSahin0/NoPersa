using Blazored.LocalStorage;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Website.Client.Enums;
using Website.Client.Models;
using Website.Client.Services;

namespace Website.Client.FormModels
{
    public class DeliveryStatusModel
    {
        [JsonIgnore]
        public ILocalStorageService LocalStorage { get; init; }

        [JsonIgnore]
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        [JsonIgnore]
        public HttpClient HttpClient { get; init; }

        [JsonIgnore]
        public NotificationService NotificationService { get; init; }

        public DeliveryStatusModel(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService)
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
        }

        [ValidateComplexType]
        [Required]
        public required List<RouteDetails> RouteDetails { get; set; }

        public bool HideNonDeliverableCustomer { get; set; } = false;

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month {  get; set; }

        [Required]
        public required int Day { get; set;}

        public async Task OnDayMonthYearSelected()
        {
            try
            {
                using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/DeliveryManagement/GetDeliveryStatus",
                    new
                    {
                        Year = Year,
                        Month = Month,
                        Day = Day,
                    }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    RouteDetails = [.. JsonSerializer.Deserialize<List<RouteDetails>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.OrderBy(x => x.Position)];
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }     
        }
    }
}
