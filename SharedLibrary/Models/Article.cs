using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        [DoubleType(min: 0)]
        public required double Price { get; set; }

        [Required]
        [DoubleType(min: 0)]
        public required double NewPrice { get; set; }

        public List<Customer> Customers { get; set; } = [];
    }
}
