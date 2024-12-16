using System.Text.Json.Serialization;

namespace NoPersaService.DTOs.Gastro.Answer
{
    public class DTOBoxContentSummary
    {
        [JsonIgnore]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public List<DTOPortionSizeSummary>? PortionSizeSummary { get; set; }

        public DTOBoxContentSummary Clone()
        {
            return new DTOBoxContentSummary { Id = Id, Name = Name, PortionSizeSummary = PortionSizeSummary?.Select(x => x.Clone()).ToList() };
        }
    }
}
