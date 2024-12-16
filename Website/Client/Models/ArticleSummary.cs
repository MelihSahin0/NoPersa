using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class ArticleSummary
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }           

        [Required(ErrorMessage = "Price is required.")]
        [DoubleType(min: 0)]
        public required string Price { get; set; }

        [Required(ErrorMessage = "NewName is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? NewName { get; set; }

        [Required(ErrorMessage = "NewPrice is required.")]
        [DoubleType(min: 0)]
        public string? NewPrice { get; set; }

        public bool IsDefault { get; set; } = false;

        [Required]
        [IntType(min: 0)]
        public required int NumberOfCustomers { get; set; }

        public bool IsDragOver { get; set; }

        public bool IsDisabled { get; set; } = true;
    }
}
