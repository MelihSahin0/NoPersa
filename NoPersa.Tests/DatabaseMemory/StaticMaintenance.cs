using Microsoft.OpenApi.Extensions;
using SharedLibrary.Models;
using SharedLibrary.Util;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticMaintenances
    {
        public static List<Maintenance> GetMaintenances() =>
        [
            new() { Id = 1, Type = MaintenanceTypes.DailyDelivery.GetDisplayName() ,Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)},
        ];
    }
}
