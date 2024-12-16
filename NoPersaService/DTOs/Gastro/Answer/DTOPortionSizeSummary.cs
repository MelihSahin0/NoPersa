using System.Text.Json.Serialization;

namespace NoPersaService.DTOs.Gastro.Answer
{
    public class DTOPortionSizeSummary
    {
        [JsonIgnore]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public int Value { get; set; }

        public DTOPortionSizeSummary Clone()
        {
            return new DTOPortionSizeSummary() { Id = Id, Name = Name, Value = Value };
        }
    }
}
