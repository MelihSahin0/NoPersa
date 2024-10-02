using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<Customer> Customers { get; set; } = [];
    }
}
