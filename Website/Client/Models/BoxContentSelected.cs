using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class BoxContentSelected
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required long PortionSizeId { get; set; }
    }
}
