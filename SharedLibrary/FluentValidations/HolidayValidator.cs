using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class HolidayValidator : AbstractValidator<Holiday>
    {
        public HolidayValidator() 
        {
            RuleFor(x => x.Country)
               .NotEmpty().WithMessage("Country is required.");

            RuleFor(x => x.Year)
               .NotNull().WithMessage("Year is required.")
               .GreaterThanOrEqualTo(1900).WithMessage("Year must be a positive number greater then 1900");

            RuleFor(x => x.Month)
               .NotNull().WithMessage("Month is required.")
               .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Day)
               .NotNull().WithMessage("Day is required.")
               .InclusiveBetween(1, 31).WithMessage("Day must be between 1 and 31.");
        }
    }
}
