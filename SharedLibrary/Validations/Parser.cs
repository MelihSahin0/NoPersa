using System.Globalization;

namespace SharedLibrary.Validations
{
    public static class Parser
    {
        public static bool ParseToDouble(string value, out double result)
        {
            if (!double.TryParse(value.ToString().Replace(",", ".").Trim(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out double number))
            {
                result = 0;
                return false;
            }

            result = number;
            return true;
        }
    }
}
