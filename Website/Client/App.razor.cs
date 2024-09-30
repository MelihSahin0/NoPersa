using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Website.Client
{
    public partial class App
    {
        [Inject]
        public required ILocalStorageService LocalStorage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LocalStorage.SetItemAsync("defaultAddress", "localhost:8081");
        }
    }
}
