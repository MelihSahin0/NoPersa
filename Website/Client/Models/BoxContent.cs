using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class BoxContent
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required List<PortionSize> PortionSizes { get; set;}
    }
}
