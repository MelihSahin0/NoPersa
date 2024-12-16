using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Models
{
    public class CustomersFoodWish
    {
        [Required]
        public required long CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        public required long FoodWishId { get; set; }

        public FoodWish? FoodWish { get; set; }
    }
}
