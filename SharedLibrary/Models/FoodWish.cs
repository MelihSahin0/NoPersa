using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class FoodWish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required bool IsIngredient { get; set; }

        public List<CustomersFoodWish> CustomersFoodWishes { get; set; } = [];

        public List<Customer> Customers { get; set; } = [];
    }
}
