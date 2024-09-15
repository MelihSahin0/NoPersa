using System.Globalization;

namespace Website.Client.Util
{
    public static class Parser
    {
        public static bool ParseToDouble(string value, out double result)
        {
            if (!double.TryParse(value.ToString(), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out double number))
            {
                if (!double.TryParse(value.ToString().Replace(",", "."), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out number))
                {
                    result = 0;
                    return false;
                }
            }

            result = number;
            return true;
        }
    }
}
