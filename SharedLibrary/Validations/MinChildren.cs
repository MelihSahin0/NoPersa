using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinChildren : ValidationAttribute
    {
        private int min;

        public MinChildren(int min = 1)
        {
            this.min = min;
        }

        public override bool IsValid(object? value)
        {
            if (value is IList list)
            {
                return list.Count >= min;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"This field requires {min} Children";
        }
    }
}
