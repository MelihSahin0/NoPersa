using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs.Gastro
{
    public class DTOBoxContentSummary
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<DTOPortionSizeSummary>? PortionSizeSummary { get; set; }

        public DTOBoxContentSummary Clone()
        {
            return new DTOBoxContentSummary { Id = Id, Name = Name, PortionSizeSummary = PortionSizeSummary?.Select(x => x.Clone()).ToList() };
        }
    }
}
