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

        public required BoxConfigurationModel BoxConfigurationModel { get; set; }

        protected override void OnInitialized()
        {
            BoxConfigurationModel = new BoxConfigurationModel() { LightDiets = [], BoxContents = [], PortionSizes = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response1 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/GetLightDiets")!;

                if (response1.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxConfigurationModel.LightDiets = JsonSerializer.Deserialize<List<DragDropInput>>(await response1.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError(await response1.Content.ReadAsStringAsync());
                }

                using var response2 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/GetBoxContents")!;

                if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxConfigurationModel.BoxContents = JsonSerializer.Deserialize<List<DragDropInput>>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                }

                using var response3 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/GetPortionSizes")!;

                if (response3.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxConfigurationModel.PortionSizes = JsonSerializer.Deserialize<List<DragDropInput>>(await response3.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
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
                using var response1 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/UpdateLightDiets", BoxConfigurationModel.LightDiets, JsonSerializerOptions);
                using var response2 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/UpdatePortionSizes", BoxConfigurationModel.PortionSizes, JsonSerializerOptions);
                using var response3 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/UpdateBoxContents", BoxConfigurationModel.BoxContents, JsonSerializerOptions);

                bool isSuccess1 = response1.StatusCode == System.Net.HttpStatusCode.OK;
                bool isSuccess2 = response2.StatusCode == System.Net.HttpStatusCode.OK;
                bool isSuccess3 = response3.StatusCode == System.Net.HttpStatusCode.OK;

                if (isSuccess1 && isSuccess2 && isSuccess3)
                {
                    NotificationService.SetSuccess("Successfully updated box configuration");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    if (!isSuccess1)
                    {
                        NotificationService.SetError(await response1.Content.ReadAsStringAsync());
                    }
                    if (!isSuccess2)
                    {
                        NotificationService.SetError(await response2.Content.ReadAsStringAsync());
                    }
                    if (!isSuccess3)
                    {
                        NotificationService.SetError(await response3.Content.ReadAsStringAsync());
                    }
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
