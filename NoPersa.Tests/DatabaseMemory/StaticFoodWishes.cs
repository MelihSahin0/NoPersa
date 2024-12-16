
using NoPersaService.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticFoodWishes
    {
        public static List<FoodWish> GetFoodWishes() =>
        [
            new() { Id = 1, Name = "Salat", IsIngredient = false, Position = 0},
            new() { Id = 2, Name = "Sugar", IsIngredient = true, Position = 1}
        ];
    }
}
