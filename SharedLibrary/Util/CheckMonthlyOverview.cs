using SharedLibrary.Models;

namespace SharedLibrary.Util
{
    public static class CheckMonthlyOverview
    {
        public static void CheckAndAdd(Customer customer, DateTime? dateTimeSuggestion = null)
        {
            DateTime dateTime = dateTimeSuggestion ?? DateTime.Today;

            if (customer.MonthlyOverviews.Any(m => m.Year == dateTime.Year && m.Month == dateTime.Month))
                return;

            MonthlyOverview monthlyOverview = new()
            {
                Customer = customer,
                Year = dateTime.Year,
                Month = dateTime.Month
            };

            List<DailyOverview> dailyOverviews = Enumerable.Range(1, 31).Select(i => new DailyOverview
            {
                DayOfMonth = i,
                Price = i < dateTime.Day ? customer.DefaultPrice : null,
                NumberOfBoxes = i < dateTime.Day ? 0 : null,
                MonthlyOverview = monthlyOverview
            }).ToList();

            monthlyOverview.DailyOverviews = dailyOverviews;
            customer.MonthlyOverviews.Add(monthlyOverview);
        }

        public static bool GetDeliveryTrueOrFalse(Customer dbCustomer, Holiday? holiday, int year, int month, int day)
        {
            DateTime date = new(year, month, day);
            string propertyName = date.DayOfWeek.ToString();

            if (dbCustomer.TemporaryDelivery)
            {
                return true;
            }
            if (dbCustomer.TemporaryNoDelivery)
            {
                return false;
            }

            if (holiday == null)
            {
                var propertyInfo = typeof(Weekday).GetProperty(propertyName);

                if (propertyInfo == null || !propertyInfo.CanRead)
                {
                    throw new InvalidOperationException($"Property '{propertyName}' does not exist or cannot be read.");
                }

                return (bool)(propertyInfo.GetValue(dbCustomer.Workdays) ?? true);
            }
            else
            {
                var propertyInfo = typeof(Weekday).GetProperty(propertyName);

                if (propertyInfo == null || !propertyInfo.CanRead)
                {
                    throw new InvalidOperationException($"Property '{propertyName}' does not exist or cannot be read.");
                }

                return (bool)(propertyInfo.GetValue(dbCustomer.Holidays) ?? true);
            }
        }
    }
}