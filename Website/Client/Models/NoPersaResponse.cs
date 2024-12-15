using System.Text.Json;
using System.Text.Json.Serialization;

namespace Website.Client.Models
{
    public class NoPersaResponse
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("detail")]
        public required string Detail { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, string>? Errors { get; set; }

        [JsonPropertyName("traceId")]
        public string? TraceId { get; set; }

        public static async Task<NoPersaResponse> Deserialize(HttpResponseMessage responseMessage)
        {
            return JsonSerializer.Deserialize<NoPersaResponse>(await responseMessage.Content.ReadAsStringAsync())!;
        }
    }
}
