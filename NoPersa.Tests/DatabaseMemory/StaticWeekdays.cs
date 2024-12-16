using NoPersaService.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticWeekdays
    {
        public static List<Weekday> GetWeekdays() =>
        [
            new() { Id = 1, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = true, Sunday = true },
            new() { Id = 2, Monday = false, Tuesday = false, Wednesday = false, Thursday = false, Friday = false, Saturday = false, Sunday = false },
        ];
    }
}
