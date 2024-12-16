using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Models
{
    public class BoxContent
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Name { get; set; }

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];
    }
}
