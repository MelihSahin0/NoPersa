using Blazored.LocalStorage;
using SharedLibrary.Util;
using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Website.Client.Exceptions;
using Website.Client.Models;
using Website.Client.Services;

namespace Website.Client.FormModels
{
    public class Customer
    {
        [JsonIgnore]
        public ILocalStorageService LocalStorage { get; init; }

        [JsonIgnore]
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        [JsonIgnore]
        public HttpClient HttpClient { get; init; }

        [JsonIgnore]
        public NotificationService NotificationService { get; init; }

        public Customer(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService)
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
        }

        [Required]
        public required int Id { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? SerialNumber { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Region is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Region { get; set; }

        [GeoCoordinatesType]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? GeoLocation { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? ContactInformation { get; set; }

        [Required]
        public bool TemporaryDelivery { get; set; }

        [Required]
        public bool TemporaryNoDelivery { get; set; }

        [Required]
        public Weekdays? Workdays { get; set; }

        [Required]
        public Weekdays? Holidays { get; set; }

        [ValidateComplexType]
        [Required(ErrorMessage = "Month is required.")]
        public required MonthOfTheYear DisplayMonth { get; set; }

        [Required]
        public required string Article { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DoubleType(min: 0)]
        public required string DefaultPrice { get; set; }

        [Required(ErrorMessage = "Number of Boxes is required")]
        [IntType(min: 0)]
        public required string DefaultNumberOfBoxes { get; set; }

        [ValidateComplexType]
        [Required(ErrorMessage = "Monthly Delivery is required")]
        public required List<MonthlyDelivery> MonthlyDeliveries { get; set; }

        [JsonIgnore]
        public bool ModifyMonthlyDelivery => DateTimeCalc.MonthDifferenceMax1(DateTime.Today.Year, MonthlyDeliveries[selectedMonthlyDeliveries].MonthOfTheYear.Year, DateTime.Today.Month, (int)MonthlyDeliveries[selectedMonthlyDeliveries].MonthOfTheYear.Month);

        [IntType(min: 0)]
        public int? RouteId { get; set; }

        [JsonIgnore]
        public List<RouteOverview>? RouteDetails { get; set; }

        [JsonIgnore]
        public int selectedMonthlyDeliveries = 0;
        public async Task OnMonthYearSelected()
        {
            int i = 0;
            foreach (MonthlyDelivery monthlyDelivery in MonthlyDeliveries)
            {
                if (monthlyDelivery.MonthOfTheYear.Month.Equals(DisplayMonth.Month) &&
                    monthlyDelivery.MonthOfTheYear.Year.Equals(DisplayMonth.Year))
                {
                    selectedMonthlyDeliveries = i;
                    return;
                }
                i++;
            }

            using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagmentService")}/CustomerManagment/GetCustomerDailyDelivery",
                                     new
                                     {
                                         ReferenceId = Id,
                                         Month = DisplayMonth.Month,
                                         Year = DisplayMonth.Year,
                                     }, JsonSerializerOptions)!;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MonthlyDeliveries.Add(JsonSerializer.Deserialize<MonthlyDelivery>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                List<DailyDelivery> dailyOverviews = [];
                for (int day = 1; day <= 31; day++)
                {
                    dailyOverviews.Add(new DailyDelivery() { DayOfMonth = day });
                }

                MonthlyDeliveries.Add(new MonthlyDelivery()
                {
                    MonthOfTheYear = new MonthOfTheYear() { Month = DisplayMonth.Month, Year = DisplayMonth.Year },
                    DailyDeliveries = dailyOverviews
                });
            }
            else
            {
                throw new ServiceNotReachableException();
            }

            selectedMonthlyDeliveries = MonthlyDeliveries.Count - 1;
        }
    }
}
