using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Name { get; set; }

        public List<Customer> Customers { get; set; } = [];
    }
}
