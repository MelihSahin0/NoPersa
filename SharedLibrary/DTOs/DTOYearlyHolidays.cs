
using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs
{
    public class DTOYearlyHolidays
    {
        [JsonPropertyName("meta")]
        public Meta? Meta { get; set; }

        [JsonPropertyName("response")]
        public Response? Response { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }

    public class Response
    {
        [JsonPropertyName("holidays")]
        public required List<Holiday> Holidays { get; set; }
    }

    public class Holiday
    {
        [JsonPropertyName("date")]
        public required DateInfo Date { get; set; }

        [JsonPropertyName("type")]
        public required List<string> Type { get; set; }
    }

    public class DateTimeDetails
    {
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; }
    }

    public class DateInfo
    {
        [JsonPropertyName("iso")]
        public required string Iso { get; set; }

        [JsonPropertyName("datetime")]
        public required DateTimeDetails Datetime { get; set; }
    }
}
