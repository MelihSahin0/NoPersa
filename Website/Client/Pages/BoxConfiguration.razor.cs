﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Components.Default;
using Website.Client.FormModels;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class BoxConfiguration
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

        public required BoxConfigurations BoxConfigurations { get; set; }

        protected override void OnInitialized()
        {
            BoxConfigurations = new BoxConfigurations() { LightDiets = [], BoxContents = [], PortionSizes = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response1 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/GetLightDiets")!;

                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxConfigurations.LightDiets = JsonSerializer.Deserialize<List<DragDropInput>>(await response1.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError(await response1.Content.ReadAsStringAsync());
                }

                using var response2 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/GetBoxContents")!;

                if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxConfigurations.BoxContents = JsonSerializer.Deserialize<List<DragDropInput>>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                }

                using var response3 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/GetPortionSizes")!;

                if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxConfigurations.PortionSizes = JsonSerializer.Deserialize<List<DragDropInput>>(await response3.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError(await response3.Content.ReadAsStringAsync());
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
                using var response1 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/UpdateLightDiets", BoxConfigurations.LightDiets, JsonSerializerOptions);
                using var response2 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/UpdateBoxContents", BoxConfigurations.BoxContents, JsonSerializerOptions);
                using var response3 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/UpdatePortionSizes", BoxConfigurations.PortionSizes, JsonSerializerOptions);

                if (response1.StatusCode == System.Net.HttpStatusCode.OK && response2.StatusCode == System.Net.HttpStatusCode.OK && response3.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated box configuration");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    NotificationService.SetError(await response1.Content.ReadAsStringAsync());
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
