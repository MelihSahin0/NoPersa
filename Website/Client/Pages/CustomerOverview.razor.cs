using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.FormModels;
using Website.Client.Util;

namespace Website.Client.Pages
{
    public partial class CustomerOverview
    {
        private Customer? Customer {  get; set; }

        [Inject]
        private HttpClient? HttpClient { get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Now;

            Customer ??= new()
            {
                SerialNumber = string.Empty,
                Name = string.Empty,
                Title = string.Empty,
                Address = string.Empty,
                Region = string.Empty,
                GeoLocation = string.Empty,
                Month = new Customer.MonthOfTheYear() 
                { 
                    Month = (Months)dateTime.Month, 
                    Year = dateTime.Year.ToString() 
                },
                Article = "0",
                Price = "0",
                DefaultNumberOfBoxes = "0",
                MonthlyDeliveries = [
                    new Customer.MonthlyDelivery()
                    {
                        MonthOfTheYear = new Customer.MonthOfTheYear()
                        {
                                Month = (Months)dateTime.Month,
                                Year = dateTime.Year.ToString()
                        },
                        Day1 = new Customer.DailyDelivery(),
                        Day2 = new Customer.DailyDelivery(),
                        Day3 = new Customer.DailyDelivery(),
                        Day4 = new Customer.DailyDelivery(),
                        Day5 = new Customer.DailyDelivery(),
                        Day6 = new Customer.DailyDelivery(),
                        Day7 = new Customer.DailyDelivery(),
                        Day8 = new Customer.DailyDelivery(),
                        Day9 = new Customer.DailyDelivery(),
                        Day10 = new Customer.DailyDelivery(),
                        Day11 = new Customer.DailyDelivery(),
                        Day12 = new Customer.DailyDelivery(),
                        Day13 = new Customer.DailyDelivery(),
                        Day14 = new Customer.DailyDelivery(),
                        Day15 = new Customer.DailyDelivery(),
                        Day16 = new Customer.DailyDelivery(),
                        Day17 = new Customer.DailyDelivery(),
                        Day18 = new Customer.DailyDelivery(),
                        Day19 = new Customer.DailyDelivery(),
                        Day20 = new Customer.DailyDelivery(),
                        Day21 = new Customer.DailyDelivery(),
                        Day22 = new Customer.DailyDelivery(),
                        Day23 = new Customer.DailyDelivery(),
                        Day24 = new Customer.DailyDelivery(),
                        Day25 = new Customer.DailyDelivery(),
                        Day26 = new Customer.DailyDelivery(),
                        Day27 = new Customer.DailyDelivery(),
                        Day28 = new Customer.DailyDelivery(),
                        Day29 = new Customer.DailyDelivery(),
                        Day30 = new Customer.DailyDelivery(),
                        Day31 = new Customer.DailyDelivery()
                    }
                ],
                Workdays = new Weekdays(),
                Holidays = new Weekdays(),
                ContactInformation = string.Empty
            };
        }

        private async Task Submit(EditContext editContext)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };

            using var response = await HttpClient?.PostAsJsonAsync("https://localhost:8081/CustomerManagment/CustomerAdd", Customer, options);
            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
