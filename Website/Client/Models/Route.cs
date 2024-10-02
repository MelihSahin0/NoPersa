using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class Route
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required List<CustomersRoute> CustomersRoute { get; set; }

        public bool IsDragOver { get; set; }
    }

    public class CustomersRoute
    {
        [Required]
        public required int Id { get; init; }

        [Required]
        public required int Position { get; set; }

        [Required]
        public required string Name { get; init; }

        public bool IsDragOver { get; set; }
    }
}
