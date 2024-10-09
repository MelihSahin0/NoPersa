
namespace SharedLibrary.Util
{
    public static class DateTimeCalc
    {
        public static bool MonthDifferenceMax1(int year1, int year2, int month1, int month2)
        {
            return (year1 - year2) * 12 + (month1 - month2) < 2;
        }
    }
}
