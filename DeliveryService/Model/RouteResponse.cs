using System.Text.Json.Serialization;

namespace DeliveryService.Model
{
    public class RouteResponse
    {
        [JsonPropertyName("formatVersion")]
        public string? FormatVersion { get; set; }

        [JsonPropertyName("routes")]
        public required Route[] Routes { get; set; }
    }

    public class Route
    {
        [JsonPropertyName("summary")]
        public required Summary Summary { get; set; }

        [JsonPropertyName("legs")]
        public required Leg[] Legs { get; set; }
    }

    public class Summary
    {
        [JsonPropertyName("lengthInMeters")]
        public double LengthInMeters { get; set; }

        [JsonPropertyName("travelTimeInSeconds")]
        public double TravelTimeInSeconds { get; set; }

        [JsonPropertyName("trafficDelayInSeconds")]
        public double TrafficDelayInSeconds { get; set; }

        [JsonPropertyName("trafficLengthInMeters")]
        public double TrafficLengthInMeters { get; set; }

        [JsonPropertyName("departureTime")]
        public string? DepartureTime { get; set; }

        [JsonPropertyName("arrivalTime")]
        public string? ArrivalTime { get; set; }
    }

    public class Leg
    {
        [JsonPropertyName("summary")]
        public required Summary Summary { get; set; }

        [JsonPropertyName("points")]
        public required List<Point> Points { get; set; }

    }

    public class Point
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }
}
