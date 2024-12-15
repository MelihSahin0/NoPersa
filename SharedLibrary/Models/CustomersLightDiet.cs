using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class CustomersLightDiet
    {    
        [Required]
        public required long CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        public required long LightDietId { get; set; }

        public LightDiet? LightDiet { get; set; }
    }
}
