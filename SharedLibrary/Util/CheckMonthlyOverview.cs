using SharedLibrary.Models;
using System;

namespace SharedLibrary.Util
{
    public static class CheckMonthlyOverview
    {
        public static void CheckAndAdd(Customer customer, DateTime? generateMonth = null)
        {
            DateTime dateTime = generateMonth ?? DateTime.Today;

            if (customer.MonthlyOverviews.Any(m => m.Year == dateTime.Year && m.Month == dateTime.Month))
                return;

            customer.MonthlyOverviews.Add(Generate(customer, dateTime));
        }

        public static MonthlyOverview Generate(Customer? customer, DateTime generateMonth)
        {
            MonthlyOverview monthlyOverview = new()
            {
                Customer = customer,
                Year = generateMonth.Year,
                Month = generateMonth.Month
            };

            List<DailyOverview> dailyOverviews = Enumerable.Range(1, DateTime.DaysInMonth(generateMonth.Year, generateMonth.Month)).Select(i =>
            {
                var today = DateTime.Today;
                return new DailyOverview
                {
                    DayOfMonth = i,
                    Price = generateMonth < today ? customer == null ? 0 : customer.Article.Price : null,
                    NumberOfBoxes = generateMonth < today ? 0 : null,
                    MonthlyOverview = monthlyOverview
                };
            }).ToList();

            monthlyOverview.DailyOverviews = dailyOverviews;

            return monthlyOverview;
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