
using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticCustomersFoodWishes
    {
        public static List<CustomersFoodWish> GetCustomersFoodWishes() =>
        [
            new() { CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!, FoodWishId = 1, FoodWish = StaticFoodWishes.GetFoodWishes().FirstOrDefault(c => c.Id == 1)!},
            new() { CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!, FoodWishId = 2, FoodWish = StaticFoodWishes.GetFoodWishes().FirstOrDefault(c => c.Id == 2)!},
            new() { CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!, FoodWishId = 1, FoodWish = StaticFoodWishes.GetFoodWishes().FirstOrDefault(c => c.Id == 1)!},
            new() { CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!, FoodWishId = 2, FoodWish = StaticFoodWishes.GetFoodWishes().FirstOrDefault(c => c.Id == 2)!},
        ];
    }
}
