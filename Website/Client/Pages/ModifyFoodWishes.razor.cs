using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using Website.Client.Components.Default;
using Website.Client.FormModels;
using Website.Client.Models;
using Website.Client.Services;

namespace Website.Client.Pages
{
    public partial class ModifyFoodWishes
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

        public required ModifyFoodWishesModel ModifyFoodWishesModel { get; set; }

        protected override void OnInitialized()
        {
            ModifyFoodWishesModel = new ModifyFoodWishesModel() { FoodWishes = [], IngredientWishes = [] };
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                using var response = await HttpClient.GetAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/GetFoodWishes");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                   
                    var foodWishes = JsonSerializer.Deserialize<List<FoodWishes>>(await response.Content.ReadAsStringAsync(), JsonSerializerOptions)!;
                    ModifyFoodWishesModel.FoodWishes = foodWishes.Where(f => !f.IsIngredient).Select(f => new DragDropInput() { Id = f.Id, Position = f.Position, Value = f.Name }).ToList();
                    ModifyFoodWishesModel.IngredientWishes = foodWishes.Where(f => f.IsIngredient).Select(f => new DragDropInput() { Id = f.Id, Position = f.Position, Value = f.Name }).ToList();
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
                List<FoodWishes> foodWishes = [];
                foodWishes.AddRange(ModifyFoodWishesModel.FoodWishes.Select(f => new FoodWishes() { Id = f.Id, Position = f.Position, Name = f.Value, IsIngredient = false }));
                foodWishes.AddRange(ModifyFoodWishesModel.IngredientWishes.Select(f => new FoodWishes() { Id = f.Id, Position = f.Position, Name = f.Value, IsIngredient = true }));
                using var response = await HttpClient.PostAsJsonAsync($"https://{await LocalStorage.GetItemAsync<string>(ServiceNames.NoPersaService.ToString())}/GastronomyManagement/UpdateFoodWishes", foodWishes, JsonSerializerOptions);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.SetSuccess("Successfully updated foodWishes");
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
