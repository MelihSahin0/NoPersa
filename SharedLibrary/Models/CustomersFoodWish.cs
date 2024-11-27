namespace SharedLibrary.Models
{
    public class CustomersFoodWish
    {
        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public int FoodWishId { get; set; }

        public required FoodWish FoodWish { get; set; }
    }
}
