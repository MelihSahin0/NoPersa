using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.FormModels;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class DeliveryStatus
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

        public required DeliveryStatusModel DeliveryStatusModel { get; set; }

        public required int TabIndex { get; set; }
        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Today;
            DeliveryStatusModel = new DeliveryStatusModel(LocalStorage, JsonSerializerOptions, HttpClient, NotificationService)
            {
                RouteDetails = [],
                Year = dateTime.Year,
                Month = (Enums.Months)(dateTime.Month),
                Day = dateTime.Day,
            };
            TabIndex = 0;         
        }

        protected override async Task OnInitializedAsync()
        {
            await DeliveryStatusModel.OnDayMonthYearSelected();
        }

        private void OpenCustomerOverview(string customerId)
        {
            NavigationContainer.CustomerId = customerId;
            NavigationManager.NavigateTo("/customer");
        }
    }
}
