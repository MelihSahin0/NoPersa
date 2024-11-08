using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs.Gastro
{
    public class DTOPortionSizeSummary
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Value { get; set; }

        public DTOPortionSizeSummary Clone()
        {
            return new DTOPortionSizeSummary() { Id = Id, Name = Name, Value = Value };
        }
    }
}
