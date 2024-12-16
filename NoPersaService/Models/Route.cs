using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Models
{
    public class Route
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Name { get; set; }

        [Required]
        public bool IsDrivable { get; set; }

        public List<Customer> Customers { get; set; } = [];
    }
}
