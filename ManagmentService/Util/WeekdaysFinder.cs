using ManagmentService.Database;
using ManagmentService.DTOs;
using SharedLibrary.Models;

namespace ManagmentService.Util
{
    public static class WeekdaysFinder
    {
        public static void NewWeekdays(NoPersaDbContext context, DTOWeekdays? customerWorkdays, DTOWeekdays? customerHolidays, out Weekdays workdays, out Weekdays holidays)
        {
            workdays = null;
            holidays = null;

            foreach (var weekday in context.Weekdays)
            {
                if (weekday.Equals(customerWorkdays))
                {
                    workdays = weekday;
                }

                if (weekday.Equals(customerHolidays))
                {
                    holidays = weekday;
                }

                if (workdays != null && holidays != null)
                {
                    return;
                }
            }

            if (workdays == null && holidays == null)
            {
                if (customerWorkdays.Equals(customerHolidays))
                {
                    Weekdays weekdays = new()
                    {
                        Monday = customerWorkdays?.Monday ?? false,
                        Tuesday = customerWorkdays?.Tuesday ?? false,
                        Wednesday = customerWorkdays?.Wednesday ?? false,
                        Thursday = customerWorkdays?.Thursday ?? false,
                        Friday = customerWorkdays?.Friday ?? false,
                        Saturday = customerWorkdays?.Saturday ?? false,
                        Sunday = customerWorkdays?.Sunday ?? false,
                    };
                    workdays = weekdays;
                    holidays = weekdays;

                    return;
                }
            }

            if (workdays == null)
            {
                workdays = new()
                {
                    Monday = customerWorkdays?.Monday ?? false,
                    Tuesday = customerWorkdays?.Tuesday ?? false,
                    Wednesday = customerWorkdays?.Wednesday ?? false,
                    Thursday = customerWorkdays?.Thursday ?? false,
                    Friday = customerWorkdays?.Friday ?? false,
                    Saturday = customerWorkdays?.Saturday ?? false,
                    Sunday = customerWorkdays?.Sunday ?? false,
                };
            }

            if (holidays == null)
            {
                workdays = new()
                {
                    Monday = customerHolidays?.Monday ?? false,
                    Tuesday = customerHolidays?.Tuesday ?? false,
                    Wednesday = customerHolidays?.Wednesday ?? false,
                    Thursday = customerHolidays?.Thursday ?? false,
                    Friday = customerHolidays?.Friday ?? false,
                    Saturday = customerHolidays?.Saturday ?? false,
                    Sunday = customerHolidays?.Sunday ?? false,
                };
            }
        }
    }
}
