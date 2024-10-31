using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class BoxContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];
    }
}
