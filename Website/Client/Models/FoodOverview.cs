using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class FoodOverview
    {
        [Required]
        public required string RouteName { get; set; }

        [Required]
        public required List<BoxContentSummary> BoxContentSummary { get; set; }

        [Required]
        public required List<LightDietSummary> LightDietSummary { get; set; }
    }

    public class BoxContentSummary
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required List<PortionSizeSummary> PortionSizeSummary { get; set; }
    }

    public class PortionSizeSummary
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public int Value { get; set; }
    }

    public class LightDietSummary
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public int Value { get; set; }
    }
}
