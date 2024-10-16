﻿using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticRoutes
    {
        public static List<Route> GetRoutes() =>
        [
            new() { Id = int.MinValue, Name = "Archive", Position = 0},
            new() { Id = 1, Name = "Route 1", Position = 1},
            new() { Id = 2, Name = "Route 2", Position = 2 },
            new() { Id = 3, Name = "Route 3", Position = 3 },
        ];
    }
}
