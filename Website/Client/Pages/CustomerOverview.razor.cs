using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Components.Default;
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

        public bool IsSubmitting { get; set; } = false;

        public required CustomerOverviewModel CustomerOverviewModel { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += OnLocationChanged;

            DateTime dateTime = DateTime.Today;

            List<DailyDelivery> dailyOverviews = [];
            for (int day = 1; day <= DateTime.DaysInMonth(dateTime.Year, dateTime.Month); day++)
            {
                dailyOverviews.Add(new DailyDelivery()
                {
                    DayOfMonth = day,
                    Price = day < dateTime.Day ? "0" : "",
                    NumberOfBoxes = day < dateTime.Day ? "0" : ""
                });
            }

            CustomerOverviewModel = new(LocalStorage, JsonSerializerOptions, HttpClient, NotificationService)
            {
                Id = 0,
                SerialNumber = string.Empty,
                Name = string.Empty,
                Title = string.Empty,
                DeliveryLocation = new DeliveryLocation() 
                { 
                    Address = string.Empty,
                    Region = string.Empty,
                    GeoLocation = string.Empty,
                    DeliveryWhishes = string.Empty
                },
                DisplayMonth = new MonthOfTheYear()
                {
                    Month = (Months)(dateTime.Month),
                    Year = dateTime.Year
                },
                ArticleId = 0,
                Articles = [],
                ArticlesPrice = [],
                DefaultNumberOfBoxes = "1",
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
                RouteDetails = [],
                LightDietOverviews = [],
                PortionSizes = [],
                BoxContentSelectedList = []
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

                    using var response1 = await HttpClient?.PostAsJsonAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/CustomerManagement/GetCustomer", data, JsonSerializerOptions)!;

                    if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        CustomerOverviewModel customer = JsonSerializer.Deserialize<CustomerOverviewModel>(await response1.Content.ReadAsStringAsync(), JsonSerializerOptions)!;

                        CustomerOverviewModel.Id = customer.Id;
                        CustomerOverviewModel.SerialNumber = customer.SerialNumber;
                        CustomerOverviewModel.Title = customer.Title;
                        CustomerOverviewModel.Name = customer.Name;
                        CustomerOverviewModel.DeliveryLocation.Address = customer.DeliveryLocation.Address;
                        CustomerOverviewModel.DeliveryLocation.Region = customer.DeliveryLocation.Region;
                        CustomerOverviewModel.DeliveryLocation.GeoLocation = customer.DeliveryLocation.GeoLocation;
                        CustomerOverviewModel.DeliveryLocation.DeliveryWhishes = customer.DeliveryLocation.DeliveryWhishes;
                        CustomerOverviewModel.ContactInformation = customer.ContactInformation;
                        CustomerOverviewModel.ArticleId = customer.ArticleId;
                        CustomerOverviewModel.DefaultNumberOfBoxes = customer.DefaultNumberOfBoxes;
                        CustomerOverviewModel.MonthlyDeliveries =[.. customer.MonthlyDeliveries];
                        CustomerOverviewModel.TemporaryDelivery = customer.TemporaryDelivery;
                        CustomerOverviewModel.TemporaryNoDelivery = customer.TemporaryNoDelivery;
                        CustomerOverviewModel.Workdays = customer.Workdays;
                        CustomerOverviewModel.Holidays = customer.Holidays;
                        CustomerOverviewModel.RouteId = customer.RouteId;
                        CustomerOverviewModel.LightDietOverviews = [.. customer.LightDietOverviews];
                        CustomerOverviewModel.PortionSizes = [.. customer.PortionSizes];
                        CustomerOverviewModel.BoxContentSelectedList = [.. customer.BoxContentSelectedList];
                    }
                    else if (response1.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {}
                    else
                    {
                        NotificationService.SetError(await response1.Content.ReadAsStringAsync());
                    }
                }
                else
                {
                    using var response1 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/CustomerManagement/GetLightDiets")!;

                    if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<SelectedLightDiet> lightDietOverviews = JsonSerializer.Deserialize<List<SelectedLightDiet>>(await response1.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                        CustomerOverviewModel.LightDietOverviews = [.. lightDietOverviews];
                    }
                    else
                    {
                        NotificationService.SetError(await response1.Content.ReadAsStringAsync());
                    }

                    using var response2 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/CustomerManagement/GetBoxContentOverview")!;

                    if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        BoxContentOverview boxContentOverview = JsonSerializer.Deserialize<BoxContentOverview>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                        CustomerOverviewModel.BoxContentSelectedList = [.. boxContentOverview.BoxContentSelectedList];
                        CustomerOverviewModel.PortionSizes = [.. boxContentOverview.SelectInputs];
                    }
                    else
                    {
                        NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                    }
                }

                using var responseRoute = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/CustomerManagement/GetRoutesOverview")!;

                if (responseRoute.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CustomerOverviewModel.RouteDetails = [.. JsonSerializer.Deserialize<List<SelectInput>>(await responseRoute.Content.ReadAsStringAsync(), JsonSerializerOptions)!.OrderBy(x => x.Value)];
                }
                else
                {
                    NotificationService.SetError(await responseRoute.Content.ReadAsStringAsync());
                }

                using var responseArticle = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/ArticleManagement/GetArticlesForCustomer")!;

                if (responseRoute.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var articles = JsonSerializer.Deserialize<List<ArticlesForCustomer>>(await responseArticle.Content.ReadAsStringAsync(), JsonSerializerOptions)!;

                    CustomerOverviewModel.Articles = articles.Select(a => new SelectInput() { Id = a.Id, Value = a.Name}).ToList();
                    CustomerOverviewModel.ArticlesPrice = articles;

                    if (CustomerOverviewModel.ArticleId == 0)
                    {
                        CustomerOverviewModel.ArticleId = articles.FirstOrDefault()?.Id ?? 0;
                    }
                }
                else
                {
                    NotificationService.SetError(await responseArticle.Content.ReadAsStringAsync());
                }
            }
            catch
            { 
                NotificationService.SetError("Server is not reachable.");
            }
        }

        private async Task Submit(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                using var response = CustomerOverviewModel.Id == 0 ?
                   await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("ManagementService")}/CustomerManagement/InsertCustomer", CustomerOverviewModel, JsonSerializerOptions)
                   :
                   await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("ManagementService")}/CustomerManagement/UpdateCustomer", CustomerOverviewModel, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NavigationContainer.CustomerId = null;
                    NotificationService.SetSuccess("Successfully added/updated customer");
                    NavigationManager.NavigateTo("/");
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
            IsSubmitting = false;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
        {
            NavigationContainer.CustomerId = null;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
