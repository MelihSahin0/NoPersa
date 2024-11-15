using Microsoft.JSInterop;
using System.Text.Json;

namespace Website.Client.Services
{
    public class LeafletService
    {
        private readonly IJSRuntime jSRuntime; 
        private double[][] coordinates;

        public LeafletService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public bool CoordinatesSet()
        {
            return coordinates != null && coordinates.Length == 0;
        }

        public void SetCoordinates(double[][] coordinates)
        {
            this.coordinates = coordinates;
        }

        public async Task Init(double latitude, double longitude, double startZoom, string? url)
        {
            if (coordinates.Length == 0)
            {
                throw new ArgumentNullException("Coordinates not set");
            }

            if (!await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                var jsonCoordinates = JsonSerializer.Serialize(coordinates);
                await jSRuntime.InvokeVoidAsync("initialize", "map", latitude, longitude, startZoom, url, jsonCoordinates);
            }
        }

        public async Task AddMarker(List<Marker> markers)
        {
            foreach (var marker in markers)
            {
                await jSRuntime.InvokeVoidAsync("addMarker", marker.Latitude, marker.Longitude, marker.PopupText, marker.ImageId);
            }
        }

        public async Task ClearMarkers()
        {
            await jSRuntime.InvokeVoidAsync("clearMarkers");
        }

        public async Task AddMyMarker(Marker marker)
        {
            await jSRuntime.InvokeVoidAsync("addMyMarker", marker.Latitude, marker.Longitude, marker.PopupText);
        }

        public async Task ClearMyMarker()
        {
            await jSRuntime.InvokeVoidAsync("clearMyMarker");
        }

        public async Task DrawAllRoute()
        {
            var jsonCoordinates = JsonSerializer.Serialize(coordinates);
            await jSRuntime.InvokeVoidAsync("drawRoute", jsonCoordinates);
        }

        public class Marker
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }

            public string? PopupText { get; set; }

            public int ImageId { get; set; }
        }
    }
}
