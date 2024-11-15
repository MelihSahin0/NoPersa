﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using Website.Client.FormModels;
using Website.Client.Services;
using Website.Client.Models;

namespace Website.Client.Pages
{
    public partial class ModifyRoutes
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

        public bool IsSubmitting { get; set; } = false;

        public required ModifyRoutesModel ModifyRoutesModel { get; set; }

        protected override void OnInitialized()
        {
            ModifyRoutesModel = new ModifyRoutesModel() { RouteSummary = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/GetRoutesSummary")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModifyRoutesModel.RouteSummary = JsonSerializer.Deserialize<List<RouteSummary>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
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
            IsSubmitting = true;
            try
            {
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("DeliveryService")}/DeliveryManagement/UpdateRoutes", ModifyRoutesModel.RouteSummary, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully added/updated route(s)");
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

        private void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
