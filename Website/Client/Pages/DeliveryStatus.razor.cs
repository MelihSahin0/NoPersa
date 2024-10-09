using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.Exceptions;
using Website.Client.FormModels;

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
        public required NotificationService NotificationService { get; set; }

        public required DeliveryStats DeliveryStats { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Now;
            DeliveryStats = new DeliveryStats(LocalStorage, JsonSerializerOptions, HttpClient, NotificationService)
            {
                RouteDetails = [],
                Year = dateTime.Year,
                Month = (Enums.Months)(dateTime.Month-1),
                Day = dateTime.Day,
            };
            TabIndex = 0;         
        }

        protected override async Task OnInitializedAsync()
        {
            await DeliveryStats.OnDayMonthYearSelected();
        }

        public required int TabIndex { get; set; }
    }
}
