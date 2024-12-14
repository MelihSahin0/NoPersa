using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class RouteDetails
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required List<CustomerDeliveryStatus> CustomerDeliveryStatus { get; set; }
    }

    public class CustomerDeliveryStatus
    {
        [Required]
        public required long Id { get; set; }

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
