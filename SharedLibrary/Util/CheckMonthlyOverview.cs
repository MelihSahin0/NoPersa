using SharedLibrary.Models;

namespace SharedLibrary.Util
{
    public static class CheckMonthlyOverview
    {
        public static void CheckAndAdd(Customer customer)
        {
            DateTime dateTime = DateTime.Today;
            if (!customer.MonthlyOverviews.Any(m => m.Year == dateTime.Year && m.Month == dateTime.Month))
            {
                MonthlyOverview monthlyOverview = new()
                {
                    Customer = customer,
                    Year = dateTime.Year,
                    Month = dateTime.Month
                };

                List<DailyOverview> dailyOverviews = [];
                for (int i = 1; i <= 31; i++)
                {
                    DailyOverview dailyOverview = new()
                    {
                        DayOfMonth = i,
                        Price = customer.DefaultPrice,
                        NumberOfBoxes = i < dateTime.Day ? 0 : null,
                        MonthlyOverview = monthlyOverview
                    };
                }

                monthlyOverview.DailyOverviews = dailyOverviews;
                customer.MonthlyOverviews.Add(monthlyOverview);
            }
        }
    }
}