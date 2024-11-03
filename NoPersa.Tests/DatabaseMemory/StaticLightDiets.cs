using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticLightDiets
    {
        public static List<LightDiet> GetLightDiets() =>
        [
            new() { Id = 1, Name = "Vegan"},
            new() { Id = 2, Name = "Diabetes"}
        ];
    }
}
