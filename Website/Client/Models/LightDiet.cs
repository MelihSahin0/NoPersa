using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class LightDiet
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public int Value { get; set; }
    }
}
