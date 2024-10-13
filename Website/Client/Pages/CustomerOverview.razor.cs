using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Enums;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;

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

        [Inject]
        public required NavigationContainer NavigationContainer { get; set; }

        public required Customer Customer { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Now;

            List<DailyDelivery> dailyOverviews = [];
            for (int day = 1; day <= 31; day++)
            {
                dailyOverviews.Add(new DailyDelivery()
                {
                    DayOfMonth = day,
                    Price = day < dateTime.Day ? "0" : "",
                    NumberOfBoxes = day < dateTime.Day ? "0" : ""
                });
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
                    Month = (Months)(dateTime.Month),
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
                                Month = (Months)(dateTime.Month),
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
                RouteId = int.MinValue,
                RouteDetails = []
            };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                if (NavigationContainer.CustomerId != null)
                {
                    var data = new
                    {
                        Id = NavigationContainer.CustomerId
                    };
                    NavigationContainer.CustomerId = null;

                    using var response1 = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/CustomerManagement/GetCustomer", data, JsonSerializerOptions)!;

                    if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Customer customer = JsonSerializer.Deserialize<Customer>(await response1.Content.ReadAsStringAsync(), JsonSerializerOptions)!;

                        Customer.Id = customer.Id;
                        Customer.SerialNumber = customer.SerialNumber;
                        Customer.Title = customer.Title;
                        Customer.Name = customer.Name;
                        Customer.Address = customer.Address;
                        Customer.Region = customer.Region;
                        Customer.GeoLocation = customer.GeoLocation;
                        Customer.ContactInformation = customer.ContactInformation;
                        Customer.Article = customer.Article;
                        Customer.DefaultPrice = customer.DefaultPrice;
                        Customer.DefaultNumberOfBoxes = customer.DefaultNumberOfBoxes;
                        Customer.MonthlyDeliveries = customer.MonthlyDeliveries;
                        Customer.TemporaryDelivery = customer.TemporaryDelivery;
                        Customer.TemporaryNoDelivery = customer.TemporaryNoDelivery;
                        Customer.Workdays = customer.Workdays;
                        Customer.Holidays = customer.Holidays;
                        Customer.RouteId = customer.RouteId;
                    }
                    else if (response1.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {}
                    else
                    {
                        NotificationService.SetError(await response1.Content.ReadAsStringAsync());
                    }
                }

                using var response2 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetRoutesOverview")!;

                if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Customer.RouteDetails = [.. JsonSerializer.Deserialize<Route>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!.RouteOverview.OrderBy(x => x.Name)];
                }
                else
                {
                    NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }

        private async Task Submit(EditContext editContext)
        {
            using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("ManagementService")}/CustomerManagement/UpdateCustomer", Customer, JsonSerializerOptions);

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
