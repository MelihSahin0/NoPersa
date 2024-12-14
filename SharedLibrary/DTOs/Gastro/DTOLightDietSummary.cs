using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs.Gastro
{
    public class DTOLightDietSummary
    {
        [JsonIgnore]
        public long Id { get; set; }

        public string? Name { get; set; }

        public int Value { get; set; }

        public DTOLightDietSummary Clone()
        {
            return new DTOLightDietSummary { Id = Id, Name = Name, Value = Value };
        }
    }
}
