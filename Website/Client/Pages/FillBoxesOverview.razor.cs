using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Website.Client.FormModels;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class FillBoxesOverview
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

        public required FillBoxes FillBoxes { get; set; }

        public required List<string> TypeList { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Today;
            FillBoxes = new FillBoxes(LocalStorage, JsonSerializerOptions, HttpClient, NotificationService)
            {
                FoodOverview = [],
                DisplayTypeIndex = 0,
                FoodOverviewIndex = 0,
                RouteOverviewIndex = 0,
                Year = dateTime.Year,
                Month = (Enums.Months)(dateTime.Month),
                Day = dateTime.Day,
            };
            TypeList = ["Overview", "Fill Boxes"];
        }

        protected override async Task OnInitializedAsync()
        {
            await FillBoxes.OnDayMonthYearSelected();
        }
    }
}
