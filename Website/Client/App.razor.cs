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
            await LocalStorage.SetItemAsync(ServiceNames.NoPersaService.ToString(), "localhost:8081");
        }
    }

    public enum ServiceNames
    {
        NoPersaService,
    }
}
