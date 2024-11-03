using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs
{
    public class DTOPortionSizeNumberOf
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Value { get; set; }

        public DTOPortionSizeNumberOf Clone()
        {
            return new DTOPortionSizeNumberOf() { Id = this.Id, Name = this.Name, Value = this.Value };
        }
    }
}
