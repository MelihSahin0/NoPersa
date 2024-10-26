using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class LightDiet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public List<CustomersLightDiet> CustomersLightDiets { get; set; } = [];

        public List<Customer> Customers { get; set; } = [];
    }
}
