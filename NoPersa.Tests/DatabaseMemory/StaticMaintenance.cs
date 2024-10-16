using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticMaintenances
    {
        public static List<Maintenance> GetMaintenances() =>
        [
            new() { Id = 1, NextDailyDeliverySave = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)},
        ];
    }
}
