using Microsoft.JSInterop;
using System.Reflection;
using Website.Client.Models;

namespace Website.Client.Services
{
    public class GeoLocationService : IDisposable
    {
        private readonly IJSRuntime jSRuntime;
        private int? watchId;
        private DotNetObjectReference<GeoLocationService>? dotNetObjectReference;

        public event Func<Location, Task>? OnLocationUpdated;

        public GeoLocationService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public async Task<Location?> GetCurrentLocation()
        {
            if (await jSRuntime.InvokeAsync<bool>("canUseGeolocation"))
            {
                var result = await jSRuntime.InvokeAsync<Location>("getCurrentLocation");

                return new Location
                {
                    Latitude = result.Latitude,
                    Longitude = result.Longitude
                };
            }

            return null;
        }

        [JSInvokable]
        public void UpdateLocation(Location location)
        {
            OnLocationUpdated?.Invoke(location);
        }

        public async Task StartWatchingLocationAsync()
        {
            if (await jSRuntime.InvokeAsync<bool>("canUseGeolocation"))
            {
                dotNetObjectReference = DotNetObjectReference.Create(this);

                watchId = await jSRuntime.InvokeAsync<int>("startWatchingPosition", dotNetObjectReference);
            }
        }

        public async Task StopWatchingLocationAsync()
        {
            if (watchId != null)
            {
                await jSRuntime.InvokeVoidAsync("stopWatchingPosition", watchId);
            }

            dotNetObjectReference?.Dispose();
        }

        public async void Dispose()
        {
            await StopWatchingLocationAsync();
            dotNetObjectReference?.Dispose();
        }
    }
}
