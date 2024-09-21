﻿using System.Text.RegularExpressions;

namespace Website.Client.Util
{
    public static class OnInputValidation
    {
        public static bool Double(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return Regex.IsMatch(value.ToString()!, @"^[0-9]*([.,][0-9]*)?$");
        }

        public static bool Integer(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return Regex.IsMatch(value.ToString()!, @"^[0-9]+$");
        }

        public static bool GeoCoordinates(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return Regex.IsMatch(value.ToString(), "^[0-9.,\\- ]+$");
        }
    }
}
