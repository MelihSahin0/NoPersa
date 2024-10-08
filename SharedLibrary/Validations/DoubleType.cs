﻿using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DoubleType : ValidationAttribute
    {
        private readonly double min;
        private readonly double max;
        private int errorType;

        public DoubleType(double min = double.MinValue, double max = double.MaxValue)
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

            if (!Parser.ParseToDouble(value.ToString()!, out double number))
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
                    return $"This field allows decimal numbers";
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
