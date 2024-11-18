using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Components.Default;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;
using static Website.Client.Pages.SearchCustomer;

namespace Website.Client.Pages
{
    public partial class SearchCustomer
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

        public required List<DisplayCustomer> DisplayCustomers { get; set; }

        public required string StartFilter { get; set; }

        protected override void OnInitialized()
        {
            DisplayCustomers = [];
            StartFilter = string.Empty;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {      
                using var response1 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("ManagementService")}/CustomerManagement/GetAllCustomersName")!;

                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    DisplayCustomers = JsonSerializer.Deserialize<List<DisplayCustomer>>(await response1.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }

        private void OpenCustomerOverview(int customerId)
        {
            Console.WriteLine(customerId);
            NavigationContainer.CustomerId = customerId;
            NavigationManager.NavigateTo("/customer");
        }

        public class DisplayCustomer
        {
            [Required]
            public required int Id { get; set; }

            [Required]
            public required string Name { get; set; }
        }
    }
}
