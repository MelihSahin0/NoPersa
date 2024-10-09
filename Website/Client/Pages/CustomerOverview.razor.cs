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

            List<DailyDelivery> dailyOverviews = [];
            for (int day = 1; day <= 31; day++)
            {
                dailyOverviews.Add(new DailyDelivery() { DayOfMonth = day });
            }

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
                    Month = (Months)(dateTime.Month - 1),
                    Year = dateTime.Year
                },
                Article = "0",
                DefaultPrice = "0",
                DefaultNumberOfBoxes = "0",
                MonthlyDeliveries = [
                    new MonthlyDelivery()
                    {
                        MonthOfTheYear = new MonthOfTheYear()
                        {
                                Month = (Months)(dateTime.Month - 1),
                                Year = dateTime.Year
                        },
                        DailyDeliveries = dailyOverviews
                    }
                ],
                TemporaryDelivery = false,
                TemporaryNoDelivery = false,
                Workdays = new Weekdays(),
                Holidays = new Weekdays(),
                ContactInformation = string.Empty,
                RouteId = null,
                RouteDetails = []
            };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagment/GetRoutesOverview")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Customer.RouteDetails = [.. JsonSerializer.Deserialize<Route>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!.RouteOverview.OrderBy(x => x.Name)];
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
