using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Website.Client.Services
{
    public class NotificationService : IDisposable
    {
        private readonly NavigationManager navigationManager;

        public event Action<string>? OnSuccess;
        public event Action<string>? OnError;

        public NotificationService(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
            this.navigationManager.LocationChanged += OnLocationChanged;
        }

        public void SetSuccess(string message)
        {
            OnSuccess?.Invoke(message);
        }

        public void SetError(string message)
        {
            OnError?.Invoke(message);
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            OnSuccess?.Invoke(string.Empty);
            OnError?.Invoke(string.Empty);
        }
        public void Dispose()
        {
            navigationManager.LocationChanged -= OnLocationChanged;
        }
    }

}
