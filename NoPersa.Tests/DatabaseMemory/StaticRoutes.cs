using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticRoutes
    {
        public static List<Route> GetRoutes() =>
        [
            new() { Id = long.MinValue, Name = "Archive", Position = 0, IsDrivable = true },
            new() { Id = 1, Name = "Route 1", Position = 1, IsDrivable = true},
            new() { Id = 2, Name = "Route 2", Position = 2, IsDrivable = true},
            new() { Id = 3, Name = "Route 3", Position = 3, IsDrivable = true},
        ];
    }
}
