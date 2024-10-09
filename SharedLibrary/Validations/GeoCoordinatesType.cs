using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class GeoCoordinatesType : ValidationAttribute
    {
        private int errorType;

        public GeoCoordinatesType()
        {
        }

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return true;
            }

            string[] grades = value.ToString()!.Split(",");
            if (grades.Length != 2)
            {
                errorType = 0;
                return false;
            }

            if (string.IsNullOrWhiteSpace(grades[0]) || !Parser.ParseToDouble(grades[0].ToString()!, out double number))
            {
                errorType = 1;
                return false;
            }
            if (number is < (-90) or > 90)
            {
                errorType = 1;
                return false;
            }

            if (string.IsNullOrWhiteSpace(grades[1]) || !Parser.ParseToDouble(grades[1].ToString()!, out number))
            {
                errorType = 2;
                return false;
            }
            if (number is < (-180) or > 180)
            {
                errorType = 2;
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            switch (errorType)
            {
                case 0:
                    return $"Excpected Format: 'Latitude, Longtitude'";
                case 1:
                    return $"Invalid Latitude";
                case 2:
                    return $"Invalid Longtitude";
                default:
                    break;
            }

            return base.FormatErrorMessage(name);
        }
    }
}
