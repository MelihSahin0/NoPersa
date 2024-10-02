using Blazored.LocalStorage;
using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Exceptions;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class Customer
    {
        public required ILocalStorageService LocalStorage { get; set; }

        public required JsonSerializerOptions JsonSerializerOptions { get; set; }

        public required HttpClient HttpClient { get; set; }

        public required NotificationService NotificationService { get; set; }

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

        private int selectedMonthlyDeliveries = 0;
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
                MonthlyDeliveries.Add(new MonthlyDelivery()
                {
                    MonthOfTheYear = new MonthOfTheYear() { Month = DisplayMonth.Month, Year = DisplayMonth.Year },
                    Day1 = new DailyDelivery(),
                    Day2 = new DailyDelivery(),
                    Day3 = new DailyDelivery(),
                    Day4 = new DailyDelivery(),
                    Day5 = new DailyDelivery(),
                    Day6 = new DailyDelivery(),
                    Day7 = new DailyDelivery(),
                    Day8 = new DailyDelivery(),
                    Day9 = new DailyDelivery(),
                    Day10 = new DailyDelivery(),
                    Day11 = new DailyDelivery(),
                    Day12 = new DailyDelivery(),
                    Day13 = new DailyDelivery(),
                    Day14 = new DailyDelivery(),
                    Day15 = new DailyDelivery(),
                    Day16 = new DailyDelivery(),
                    Day17 = new DailyDelivery(),
                    Day18 = new DailyDelivery(),
                    Day19 = new DailyDelivery(),
                    Day20 = new DailyDelivery(),
                    Day21 = new DailyDelivery(),
                    Day22 = new DailyDelivery(),
                    Day23 = new DailyDelivery(),
                    Day24 = new DailyDelivery(),
                    Day25 = new DailyDelivery(),
                    Day26 = new DailyDelivery(),
                    Day27 = new DailyDelivery(),
                    Day28 = new DailyDelivery(),
                    Day29 = new DailyDelivery(),
                    Day30 = new DailyDelivery(),
                    Day31 = new DailyDelivery()
                });
            }
            else
            {
                throw new Exception();
            }

            selectedMonthlyDeliveries = MonthlyDeliveries.Count - 1;
        }

        public DailyDelivery GetDailyDeliveryByDay(int day)
        {
            return day switch
            {
                1 => MonthlyDeliveries[selectedMonthlyDeliveries].Day1,
                2 => MonthlyDeliveries[selectedMonthlyDeliveries].Day2,
                3 => MonthlyDeliveries[selectedMonthlyDeliveries].Day3,
                4 => MonthlyDeliveries[selectedMonthlyDeliveries].Day4,
                5 => MonthlyDeliveries[selectedMonthlyDeliveries].Day5,
                6 => MonthlyDeliveries[selectedMonthlyDeliveries].Day6,
                7 => MonthlyDeliveries[selectedMonthlyDeliveries].Day7,
                8 => MonthlyDeliveries[selectedMonthlyDeliveries].Day8,
                9 => MonthlyDeliveries[selectedMonthlyDeliveries].Day9,
                10 => MonthlyDeliveries[selectedMonthlyDeliveries].Day10,
                11 => MonthlyDeliveries[selectedMonthlyDeliveries].Day11,
                12 => MonthlyDeliveries[selectedMonthlyDeliveries].Day12,
                13 => MonthlyDeliveries[selectedMonthlyDeliveries].Day13,
                14 => MonthlyDeliveries[selectedMonthlyDeliveries].Day14,
                15 => MonthlyDeliveries[selectedMonthlyDeliveries].Day15,
                16 => MonthlyDeliveries[selectedMonthlyDeliveries].Day16,
                17 => MonthlyDeliveries[selectedMonthlyDeliveries].Day17,
                18 => MonthlyDeliveries[selectedMonthlyDeliveries].Day18,
                19 => MonthlyDeliveries[selectedMonthlyDeliveries].Day19,
                20 => MonthlyDeliveries[selectedMonthlyDeliveries].Day20,
                21 => MonthlyDeliveries[selectedMonthlyDeliveries].Day21,
                22 => MonthlyDeliveries[selectedMonthlyDeliveries].Day22,
                23 => MonthlyDeliveries[selectedMonthlyDeliveries].Day23,
                24 => MonthlyDeliveries[selectedMonthlyDeliveries].Day24,
                25 => MonthlyDeliveries[selectedMonthlyDeliveries].Day25,
                26 => MonthlyDeliveries[selectedMonthlyDeliveries].Day26,
                27 => MonthlyDeliveries[selectedMonthlyDeliveries].Day27,
                28 => MonthlyDeliveries[selectedMonthlyDeliveries].Day28,
                29 => MonthlyDeliveries[selectedMonthlyDeliveries].Day29,
                30 => MonthlyDeliveries[selectedMonthlyDeliveries].Day30,
                31 => MonthlyDeliveries[selectedMonthlyDeliveries].Day31,
                _ => throw new InvalidDateException()
            };
        }
    }
}
