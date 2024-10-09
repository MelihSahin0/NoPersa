using Microsoft.AspNetCore.Components;
using Website.Client.Exceptions;
using Website.Client.Services;

namespace Website.Client.Layout
{
    public partial class MainLayout : IDisposable
    {
        [Inject]
        public required NotificationService NotificationService { get; set; }

        public CustomErrorBoundary? errorBoundary;

        private string? successMessage;
        private string? errorMessage;

        protected override void OnInitialized()
        {
            NotificationService.OnSuccess += ShowSuccessMessage;
            NotificationService.OnError += ShowErrorMessage;
            errorBoundary = new();
        }

        private void ShowSuccessMessage(string message)
        {
            successMessage = message;
            errorMessage = string.Empty;
            StateHasChanged(); 
        }

        private void ShowErrorMessage(string message)
        {
            errorMessage = message;
            successMessage = string.Empty;
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            NotificationService.OnSuccess -= ShowSuccessMessage;
            NotificationService.OnError -= ShowErrorMessage;
        }

        public void Dismiss()
        {
            errorMessage = string.Empty;
            successMessage = string.Empty;
            StateHasChanged();
        }
    }
}
