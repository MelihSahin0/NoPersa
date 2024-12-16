using System.Text.Json.Serialization;

namespace NoPersaService.DTOs.Management.RA
{
    public class DTOFoodWishesOverview
    {
        public string? Id { get; set; }

        public int Position { get; set; }

        public string? Name { get; set; }

        public bool Selected { get; set; }

        [JsonIgnore]
        public bool IsIngredient { get; set; }
    }
}
