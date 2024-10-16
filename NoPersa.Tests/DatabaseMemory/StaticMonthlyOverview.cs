using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticMonthlyOverview
    {
        public static List<MonthlyOverview> GetMonthlyOverviews(int year, int month) =>
        [
            new() { Id = 1, Month = month, Year = year, CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!},
            new() { Id = 2, Month = month, Year = year, CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!},
        ];
    }
}
