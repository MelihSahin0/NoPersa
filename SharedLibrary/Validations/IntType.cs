using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IntType : ValidationAttribute
    {
        private int min;
        private int max;
        private int errorType;

        public IntType(int min = int.MinValue, int max = int.MaxValue)
        {
            this.min = min;
            this.max = max;
        }

        public override bool IsValid(object? value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return true;
            }

            if (!int.TryParse(value.ToString()!.Trim(), out int number))
            {
                errorType = 0;
                return false;
            }

            if (number < min)
            {
                errorType = 1;
                return false;
            }

            if (number > max)
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
                    return $"This field allows whole numbers";
                case 1:
                    return $"The smallest allowed number is {min}";
                case 2:
                    return $"The greatest allowed number is {max}";
                default:
                    break;
            }

            return base.FormatErrorMessage(name);
        }
    }
}
