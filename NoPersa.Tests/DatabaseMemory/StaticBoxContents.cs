using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticBoxContents
    {
        public static List<BoxContent> GetBoxContents() =>
        [
            new() { Id = 1, Name = "Soup", Position = 0},
            new() { Id = 2, Name = "Dessert", Position = 1}
        ];
    }
}
