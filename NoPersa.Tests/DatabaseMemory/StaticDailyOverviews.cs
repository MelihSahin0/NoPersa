using NoPersaService.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticDailyOverviews
    {
        public static List<DailyOverview> GetDailyOverview1(MonthlyOverview monthlyOverview) =>
        [
            new() { Id = 1, DayOfMonth = 1, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview},
            new() { Id = 2, DayOfMonth = 2, Price = 10.5, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 3, DayOfMonth = 3, Price = null, NumberOfBoxes = 0, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 4, DayOfMonth = 4, Price = null, NumberOfBoxes = 1, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 5, DayOfMonth = 5, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 6, DayOfMonth = 6, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 7, DayOfMonth = 7, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 8, DayOfMonth = 8, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 9, DayOfMonth = 9, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 10, DayOfMonth = 10, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 11, DayOfMonth = 11, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 12, DayOfMonth = 12, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 13, DayOfMonth = 13, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 14, DayOfMonth = 14, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 15, DayOfMonth = 15, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 16, DayOfMonth = 16, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 17, DayOfMonth = 17, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 18, DayOfMonth = 18, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 19, DayOfMonth = 19, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 20, DayOfMonth = 20, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 21, DayOfMonth = 21, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 22, DayOfMonth = 22, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 23, DayOfMonth = 23, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 24, DayOfMonth = 24, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 25, DayOfMonth = 25, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 26, DayOfMonth = 26, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 27, DayOfMonth = 27, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 28, DayOfMonth = 28, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 29, DayOfMonth = 29, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 30, DayOfMonth = 30, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview },
            new() { Id = 31, DayOfMonth = 31, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 1, MonthlyOverview = monthlyOverview }
        ];

        public static List<DailyOverview> GetDailyOverview2(MonthlyOverview monthlyOverview) =>
     [
         new() { Id = 32, DayOfMonth = 1, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview},
            new() { Id = 33, DayOfMonth = 2, Price = 10.5, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 34, DayOfMonth = 3, Price = null, NumberOfBoxes = 0, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 35, DayOfMonth = 4, Price = null, NumberOfBoxes = 1, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 36, DayOfMonth = 5, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 37, DayOfMonth = 6, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 38, DayOfMonth = 7, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 39, DayOfMonth = 8, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 40, DayOfMonth = 9, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 41, DayOfMonth = 10, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 42, DayOfMonth = 11, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 43, DayOfMonth = 12, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 44, DayOfMonth = 13, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 45, DayOfMonth = 14, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 46, DayOfMonth = 15, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 47, DayOfMonth = 16, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 48, DayOfMonth = 17, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 49, DayOfMonth = 18, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 50, DayOfMonth = 19, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 51, DayOfMonth = 20, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 52, DayOfMonth = 21, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 53, DayOfMonth = 22, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 54, DayOfMonth = 23, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 55, DayOfMonth = 24, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 56, DayOfMonth = 25, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 57, DayOfMonth = 26, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 58, DayOfMonth = 27, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 59, DayOfMonth = 28, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 60, DayOfMonth = 29, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 61, DayOfMonth = 30, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview },
            new() { Id = 62, DayOfMonth = 31, Price = null, NumberOfBoxes = null, MonthlyOverviewId = 2, MonthlyOverview = monthlyOverview }
     ];
    }
}