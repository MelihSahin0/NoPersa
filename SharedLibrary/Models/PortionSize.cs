using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class PortionSize
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];
    }
}
