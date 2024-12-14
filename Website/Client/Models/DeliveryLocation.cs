using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class DeliveryLocation
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Address { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Region { get; set; }

        [Required]
        [GeoCoordinatesType]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string GeoLocation { get; set; }

        public string? DeliveryWhishes { get; set; }
    }
}
