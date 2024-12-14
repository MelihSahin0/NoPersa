using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class DeliveryLocation
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Address { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Region { get; set; }

        [Required]
        public required double Latitude { get; set; }

        [Required]
        public required double Longitude { get; set; }

        public string? DeliveryWhishes { get; set; }

        [ForeignKey("CustomerId")]
        public long CustomerId { get; set; }

        public required Customer Customer { get; set; }
    }
}
