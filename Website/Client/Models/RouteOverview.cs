using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class RouteOverview
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required int NumberOfCustomers { get; set; }

        public bool IsDragOver { get; set; }
    }
}
