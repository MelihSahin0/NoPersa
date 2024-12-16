using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class BoxContentSelected
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required string PortionSizeId { get; set; }
    }
}
