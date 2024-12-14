using Blazored.LocalStorage;
using SharedLibrary.Util;
using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Website.Client.Components;
using Website.Client.Components.Default;
using Website.Client.Models;
using Website.Client.Services;
using Website.Client.Util;

namespace Website.Client.FormModels
{
    public class CustomerOverviewModel
    {
        [JsonIgnore]
        public ILocalStorageService LocalStorage { get; init; }

        [JsonIgnore]
        public JsonSerializerOptions JsonSerializerOptions { get; init; }

        [JsonIgnore]
        public HttpClient HttpClient { get; init; }

        [JsonIgnore]
        public NotificationService NotificationService { get; init; }

        public CustomerOverviewModel(ILocalStorageService localStorage, JsonSerializerOptions jsonSerializerOptions, HttpClient httpClient, NotificationService notificationService)
        {
            LocalStorage = localStorage;
            JsonSerializerOptions = jsonSerializerOptions;
            HttpClient = httpClient;
            NotificationService = notificationService;
        }

        [Required]
        public required long Id { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? SerialNumber { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [ValidateComplexType]
        [Required]
        public required DeliveryLocation DeliveryLocation { get; set; }

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
        [LongType(min:1)]
        public required long ArticleId { get; set; }

        [JsonIgnore]
        public List<SelectInput>? Articles { get; set; }

        [JsonIgnore]
        public List<ArticlesForCustomer> ArticlesPrice { get; set; }

        [Required]
        [LongType(min: 1, max: 10)]
        public required long DefaultNumberOfBoxes { get; set; }

        [JsonIgnore]
        public List<SelectInput> DefaultNumbers = Misc.GetDefaultNumberOfBoxesSelection.Where(x => x.Id != 0).ToList();

        [ValidateComplexType]
        [Required(ErrorMessage = "Monthly Delivery is required")]
        public required List<MonthlyDelivery> MonthlyDeliveries { get; set; }

        public string TotalPrice()
        {
            double total = 0;
            if (MonthlyDeliveries[selectedMonthlyDeliveries] != null)
            {
                foreach (var dailyOverview in MonthlyDeliveries[selectedMonthlyDeliveries].DailyDeliveries)
                {
                    if (double.TryParse(dailyOverview.Price?.Replace(",", "."), CultureInfo.InvariantCulture, out double price))
                    {
                    }
                    else
                    {
                        price = 0;
                    }
                    if (int.TryParse(dailyOverview.NumberOfBoxes, out int number))
                    { 
                    }
                    else
                    {
                        number = 0;
                    }

                    total += price * number;
                }
            }
            return total.ToString();
        }

        [JsonIgnore]
        public bool ModifyMonthlyDelivery => DateTimeCalc.MonthDifferenceMax1(DateTime.Today.Year, MonthlyDeliveries[selectedMonthlyDeliveries].MonthOfTheYear.Year, DateTime.Today.Month, (int)MonthlyDeliveries[selectedMonthlyDeliveries].MonthOfTheYear.Month);

        [Required]
        [LongType]
        public required long RouteId { get; set; }

        [JsonIgnore]
        public List<SelectInput>? RouteDetails { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<SelectedInputCheckbox> LightDietOverviews { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<SelectedInputCheckbox> FoodWishesOverviews { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<SelectedInputCheckbox> IngredientWishesOverviews { get; set; }

        [ValidateComplexType]
        [Required]
        [MinChildren(1, ErrorMessage = "At least one box content with one portion size should be defined")]
        public required List<BoxContentSelected> BoxContentSelectedList { get; set; }

        public List<SelectInput>? PortionSizes { get; set; }

        [JsonIgnore]
        public int selectedMonthlyDeliveries = 0;
        public async Task OnMonthYearSelected()
        {
            for (int i = 0; i < MonthlyDeliveries.Count; i++)
            {
                if (MonthlyDeliveries[i].MonthOfTheYear.Month.Equals(DisplayMonth.Month) &&
                    MonthlyDeliveries[i].MonthOfTheYear.Year.Equals(DisplayMonth.Year))
                {
                    selectedMonthlyDeliveries = i;
                    return;
                }
            }

            try
            {
                using var response = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/CustomerManagement/GetCustomerDailyDelivery",
                                         new
                                         {
                                             ReferenceId = Id,
                                             Month = DisplayMonth.Month,
                                             Year = DisplayMonth.Year,
                                         }, JsonSerializerOptions)!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MonthlyDeliveries.Add(JsonSerializer.Deserialize<MonthlyDelivery>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!);
                    selectedMonthlyDeliveries = MonthlyDeliveries.Count - 1;
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
