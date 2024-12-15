using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;
using Website.Client.Enums;

namespace Website.Client.Pages
{
    public partial class ModifyArticles
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

        public required ModifyArticlesModel ModifyArticlesModel { get; set; }

        protected override void OnInitialized()
        {
            DateTime dateTime = DateTime.Today.AddDays(1);
            ModifyArticlesModel = new ModifyArticlesModel() { Articles = [], IsTaskSet = false, Year = dateTime.Year, Month = (Months)dateTime.Month, Day = dateTime.Day  };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/ArticleManagement/GetArticles")!;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModifyArticlesModel.Articles = JsonSerializer.Deserialize<List<ArticleSummary>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                }
                else
                {
                    NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                }

                using var response2 = await HttpClient?.GetAsync($"https://{await LocalStorage!.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/TaskManagement/GetArticleTask")!;
                if (response2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var model = JsonSerializer.Deserialize<SelectedDay>(await response2.Content.ReadAsStringAsync(), JsonSerializerOptions)!;

                    if (model.Year != null && model.Month != null && model.Day != null)
                    {
                        ModifyArticlesModel.Year = (int)model.Year;
                        ModifyArticlesModel.Month = (Months)model.Month;
                        ModifyArticlesModel.Day = (int)model.Day;
                        ModifyArticlesModel.IsTaskSet = true;
                    }
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

        private async Task Submit(EditContext editContext)
        {
            IsSubmitting = true;
            try
            {
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/ArticleManagement/UpdateArticles", ModifyArticlesModel.Articles, JsonSerializerOptions);

                using var response2 = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/TaskManagement/UpdateArticleTask", new 
                    {
                        Year =  ModifyArticlesModel.IsTaskSet ? ModifyArticlesModel.Year : (int?)null, 
                        Month = ModifyArticlesModel.IsTaskSet ? (int?)ModifyArticlesModel.Month : (int?)null, 
                        Day = ModifyArticlesModel.IsTaskSet ?  ModifyArticlesModel.Day : (int?)null
                    } , JsonSerializerOptions);
                
                if (response.StatusCode == System.Net.HttpStatusCode.OK && response2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated articles");
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        NotificationService.SetError((await NoPersaResponse.Deserialize(response)).Detail);
                    }
                    else
                    {
                        NotificationService.SetError((await NoPersaResponse.Deserialize(response2)).Detail);
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
