using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class FoodOverview
    {
        [Required]
        public required string RouteName { get; set; }

        [Required]
        public required List<BoxContent> BoxContents { get; set; }

        [Required]
        public required List<LightDiet> LightDiets { get; set; }
    }
}
