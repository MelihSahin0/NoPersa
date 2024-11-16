using Microsoft.JSInterop;
using System;
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
            return coordinates != null && coordinates.Length != 0;
        }

        public void SetCoordinates(double[][] coordinates)
        {
            this.coordinates = coordinates;
        }

        public async Task Init(double startZoom, string? url)
        {
            if (coordinates.Length == 0)
            {
                throw new ArgumentNullException("Coordinates not set");
            }

            if (!await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                var jsonCoordinates = JsonSerializer.Serialize(coordinates);
                await jSRuntime.InvokeVoidAsync("initialize", "map", coordinates[0][0], coordinates[0][1], startZoom, url, jsonCoordinates);
            }
        }

        public async Task ClearMap()
        {
            if (await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                await jSRuntime.InvokeVoidAsync("clearMap");
            }
        }

        public async Task AddMarker(List<Marker> markers)
        {
            if (await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                foreach (var marker in markers)
                {
                    await jSRuntime.InvokeVoidAsync("addMarker", marker.Latitude, marker.Longitude, marker.PopupText, marker.ImageId);
                }
            }
        }

        public async Task ClearMarkers()
        {
            if (await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                await jSRuntime.InvokeVoidAsync("clearMarkers");
            }
        }

        public async Task AddMyMarker(Marker marker)
        {
            if (await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                await jSRuntime.InvokeVoidAsync("addMyMarker", marker.Latitude, marker.Longitude, marker.PopupText);
            }
        }

        public async Task ClearMyMarker()
        {
            if (await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                await jSRuntime.InvokeVoidAsync("clearMyMarker");
            }
        }

        public async Task DrawAllRoute()
        {
            if (await jSRuntime.InvokeAsync<bool>("isInitialized"))
            {
                var jsonCoordinates = JsonSerializer.Serialize(coordinates);
                await jSRuntime.InvokeVoidAsync("drawRoute", jsonCoordinates);
            }
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
