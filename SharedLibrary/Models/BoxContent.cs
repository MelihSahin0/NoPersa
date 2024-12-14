using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class BoxContent
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        public required string Name { get; set; }

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];
    }
}
