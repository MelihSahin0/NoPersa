using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Article
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        public required double Price { get; set; }

        [Required]
        [MaxLength(64)]
        public required string NewName { get; set; }

        [Required]
        public required double NewPrice { get; set; }

        [Required]
        public required bool IsDefault { get; set; }

        public List<Customer> Customers { get; set; } = [];
    }
}
