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

        [Required]
        [ForeignKey("CustomerId")]
        public required long CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
