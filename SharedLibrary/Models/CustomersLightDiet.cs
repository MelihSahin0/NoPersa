using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class CustomersLightDiet
    {
        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public int LightDietId { get; set; }

        public required LightDiet LightDiet { get; set; }

        [Required]
        public required bool Selected { get; set; }
    }
}
