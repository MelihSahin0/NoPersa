using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Website.Client.Services
{
    public class NotificationService : IDisposable
    {
        private readonly NavigationManager navigationManager;
        private Timer? clearNotificationTimer;
        private int remainingSeconds;

        public event Action<string>? OnSuccess;
        public event Action<string>? OnError;
        public event Action<int>? OnCountdown;

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
            StartClearTimer();
        }

        private void StartClearTimer()
        {
            clearNotificationTimer?.Stop();
            clearNotificationTimer?.Dispose();
            clearNotificationTimer = null;

            clearNotificationTimer = new Timer(1000)
            {
                AutoReset = true
            };
            clearNotificationTimer.Elapsed += CountdownElapsed;
            clearNotificationTimer.Start();
        }

        private void CountdownElapsed(object? sender, ElapsedEventArgs e)
        {
            if (remainingSeconds > 0)
            {
                OnCountdown?.Invoke(remainingSeconds);
                remainingSeconds--;
            }
            else
            {
                ClearNotifications();
            }
        }

        private void ClearNotifications()
        {
            clearNotificationTimer?.Stop();
            clearNotificationTimer?.Dispose();
            clearNotificationTimer = null;

            OnSuccess?.Invoke(string.Empty);
            OnError?.Invoke(string.Empty);

            remainingSeconds = 5;
            OnCountdown?.Invoke(remainingSeconds);
        }

        public void Dispose()
        {
            navigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
