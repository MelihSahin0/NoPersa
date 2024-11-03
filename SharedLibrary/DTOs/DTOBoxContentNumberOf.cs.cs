using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs
{
    public class DTOBoxContentNumberOf
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<DTOPortionSizeNumberOf>? PortionSizes { get; set; }

        public DTOBoxContentNumberOf Clone()
        {
            return new DTOBoxContentNumberOf { Id = this.Id, Name = this.Name, PortionSizes = this.PortionSizes?.Select(x => x.Clone()).ToList() };
        }
    }
}
