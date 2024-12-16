using System.Text.Json.Serialization;

namespace NoPersaService.DTOs.Gastro.Answer
{
    public class DTOLightDietSummary
    {
        [JsonIgnore]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public int Value { get; set; }

        public DTOLightDietSummary Clone()
        {
            return new DTOLightDietSummary { Id = Id, Name = Name, Value = Value };
        }
    }
}
