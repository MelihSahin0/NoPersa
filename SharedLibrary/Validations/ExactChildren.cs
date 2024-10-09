using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ExactChildren : ValidationAttribute
    {
        private readonly int numberOfChildren;

        public ExactChildren(int numberOfChildren)
        {
            this.numberOfChildren = numberOfChildren;
        }

        public override bool IsValid(object? value)
        {
            if (value is IList list)
            {
                return list.Count == numberOfChildren;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"This field requires {numberOfChildren} Children";
        }
    }
}
