﻿using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticArticles
    {
        public static List<Article> GetArticles() =>
        [
            new() { Id = 1, Position = 0, Name = "101", Price = 10.5, NewName = "201", NewPrice = 20.5 },
            new() { Id = 2, Position = 0, Name = "102", Price = 10.5, NewName = "202",  NewPrice = 20.5 },
        ];
    }
}