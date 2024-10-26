using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class RouteDetails
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required List<CustomersRoute> CustomersRoute { get; set; }
    }

    public class CustomersRoute
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required bool ToDeliver { get; set; }

        public bool IsDragOver { get; set; }
    }
}
