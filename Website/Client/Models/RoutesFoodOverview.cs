using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class RoutesFoodOverview
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        public required List<CustomersFood> CustomersFoods { get; set; }
    }

    public class CustomersFood
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        public required List<string> LightDiets { get; set; }

        [Required]
        public required List<CustomersBoxContent> BoxContents { get; set; }
    }

    public class CustomersBoxContent
    {
        [Required]
        public required string BoxName { get; set; }

        [Required]
        public required string PortionSize { get; set; }
    }
}
