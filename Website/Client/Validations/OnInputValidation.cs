using System.Text.RegularExpressions;

namespace Website.Client.Validations
{
    public static class OnInputValidation
    {
        public static bool Double(string? value)
        {
            if (value == null)
            {
                return true;
            }

            return Regex.IsMatch(value.ToString()!, @"^[0-9]*([.,][0-9]*)?$");
        }

        public static bool Integer(string? value)
        {
            if (value == null)
            {
                return true;
            }

            return Regex.IsMatch(value.ToString()!, @"^[0-9]+");
        }
    }
}
