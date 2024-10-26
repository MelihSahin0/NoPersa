using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Components.Default;
using Website.Client.FormModels;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class LightDietOverview
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

        public required LightDiet LightDiets { get; set; }

        protected override void OnInitialized()
        {
            LightDiets = new LightDiet() { DragDropInputs = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/GetLightDiets")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    LightDiets.DragDropInputs = JsonSerializer.Deserialize<List<DragDropInput>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
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
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>("GastronomyService")}/GastronomyManagement/UpdateLightDiets", LightDiets.DragDropInputs, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully added/updated light diet(s)");
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
