using NoPersaService.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticLightDiets
    {
        public static List<LightDiet> GetLightDiets() =>
        [
            new() { Id = 1, Name = "Vegan", Position = 0},
            new() { Id = 2, Name = "Diabetes", Position = 1}
        ];
    }
}
