using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.FormModels;
using Website.Client.Services;
using Website.Client.Models;

namespace Website.Client.Pages
{
    public partial class BoxStatus
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
        public required UtilityService UtilityService { get; set; }

        public bool IsSubmitting { get; set; } = false;

        public required BoxStatusModel BoxStatusModel { get; set; }

        protected override void OnInitialized()
        {
            BoxStatusModel = new BoxStatusModel() { BoxStatusOverviews = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/BoxManagement/GetBoxStatus")!;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    BoxStatusModel.BoxStatusOverviews = JsonSerializer.Deserialize<List<BoxStatusOverview>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                }
            }
            catch
            {
                NotificationService.SetError("Server is not reachable.");
            }
        }

        private bool isOverflow = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            bool overflow = await UtilityService.IsOverflowActive("overflowContainer");

            if (overflow != isOverflow)
            {
                isOverflow = overflow;
                StateHasChanged();
            }
        }

        private async Task Submit(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/BoxManagement/UpdateBoxStatus", BoxStatusModel.BoxStatusOverviews, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated box status");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
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
