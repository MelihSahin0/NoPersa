using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticPortionSizes
    {
        public static List<PortionSize> GetPortionSizes() =>
        [
            new() { Id = 1, Name = "Small", Position = 0},
            new() { Id = 2, Name = "Normal", Position = 1}
        ];
    }
}
