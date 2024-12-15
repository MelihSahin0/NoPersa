using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class LightDiet
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Name { get; set; }

        public List<CustomersLightDiet> CustomersLightDiets { get; set; } = [];

        public List<Customer> Customers { get; set; } = [];
    }
}
