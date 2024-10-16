using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticHolidays
    {
        public static List<Holiday> GetHolidays() =>
        [
            new() { Id = 1, Country = "AT", Year = 2024, Month = 1, Day = 1 },
            new() { Id = 2, Country = "AT", Year = 2024, Month = 1, Day = 2 },
        ];
    }
}
