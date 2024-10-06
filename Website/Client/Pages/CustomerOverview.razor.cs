using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Enums;
using Website.Client.Exceptions;
using Website.Client.FormModels;
using Website.Client.Models;

namespace Website.Client.Pages
{
    public partial class CustomerOverview
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

        public required Customer Customer { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Now;

            Customer ??= new(LocalStorage, JsonSerializerOptions, HttpClient, NotificationService)
            {
                Id = 0,
                SerialNumber = string.Empty,
                Name = string.Empty,
                Title = string.Empty,
                Address = string.Empty,
                Region = string.Empty,
                GeoLocation = string.Empty,
                DisplayMonth = new MonthOfTheYear() 
                { 
                    Month = (Months)dateTime.Month, 
                    Year = dateTime.Year.ToString() 
                },
                Article = "0",
                DefaultPrice = "0",
                DefaultNumberOfBoxes = "0",
                MonthlyDeliveries = [
                    new MonthlyDelivery()
                    {
                        MonthOfTheYear = new MonthOfTheYear()
                        {
                                Month = (Months)dateTime.Month,
                                Year = dateTime.Year.ToString()
                        },
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
                    }
                ],
                Workdays = new Weekdays(),
                Holidays = new Weekdays(),
                ContactInformation = string.Empty,
                RouteId = null,
                Routes = []
            };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagment/GetRoutes")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Customer.Routes = (JsonSerializer.Deserialize<FormModels.Route>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!).Routes;
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

        private async Task Submit(EditContext editContext)
        {
            using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("ManagmentService")}/CustomerManagment/AddCustomer", Customer, JsonSerializerOptions);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NotificationService.SetSuccess("Successfully added/updated customer");
                NavigationManager.NavigateTo("/");
            }
            else
            {
                NotificationService.SetError(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
