namespace SharedLibrary.Models
{
    public class CustomersFoodWish
    {
        public long CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public long FoodWishId { get; set; }

        public required FoodWish FoodWish { get; set; }
    }
}
