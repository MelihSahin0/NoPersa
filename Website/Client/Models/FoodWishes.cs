using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class FoodWishes
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required bool IsIngredient { get; set; }
    }
}
