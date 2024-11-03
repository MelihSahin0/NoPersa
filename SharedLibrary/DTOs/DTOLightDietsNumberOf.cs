using System.Text.Json.Serialization;

namespace SharedLibrary.DTOs
{
    public class DTOLightDietsNumberOf
    {
        [JsonIgnore]
        public int Id {  get; set; }

        public string? Name { get; set; }

        public int Value { get; set; }

        public DTOLightDietsNumberOf Clone() 
        {
            return new DTOLightDietsNumberOf { Id = this.Id, Name = this.Name, Value = this.Value };
        }
    }
}
