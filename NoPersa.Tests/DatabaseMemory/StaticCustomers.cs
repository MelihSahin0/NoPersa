using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticCustomers
    {
        public static List<Customer> GetCustomers() =>
        [
            new() { Id = 1, Name = "Customer 1", ArticleId = 1, Article = StaticArticles.GetArticles().First(a => a.Id == 1), DefaultNumberOfBoxes = 1, TemporaryDelivery = true, TemporaryNoDelivery = false, WorkdaysId = 1, Workdays = StaticWeekdays.GetWeekdays().First(w => w.Id == 1), HolidaysId = 1, Holidays = StaticWeekdays.GetWeekdays().First(w => w.Id == 1), RouteId = 1, Route = StaticRoutes.GetRoutes().First(r => r.Id == 1), Position = 0},
            new() { Id = 2, Name = "Customer 2", ArticleId = 2, Article = StaticArticles.GetArticles().First(a => a.Id == 2), DefaultNumberOfBoxes = 1, TemporaryDelivery = false, TemporaryNoDelivery = true, WorkdaysId = 2, Workdays = StaticWeekdays.GetWeekdays().First(w => w.Id == 2), HolidaysId = 2, Holidays = StaticWeekdays.GetWeekdays().First(w => w.Id == 2), RouteId = 2, Route = StaticRoutes.GetRoutes().First(r => r.Id == 2), Position = 0},
        ];
    }
}
