using FluentValidation;
using NoPersaService.Models;

namespace NoPersaService.FluentValidations
{
    public class WeekdayValidator : AbstractValidator<Weekday>
    {
        public WeekdayValidator() 
        {
            RuleFor(x => x.Monday)
                .NotNull().WithMessage("Monday is required.");

            RuleFor(x => x.Tuesday)
                .NotNull().WithMessage("Tuesday is required.");

            RuleFor(x => x.Wednesday)
                .NotNull().WithMessage("Wednesday is required.");

            RuleFor(x => x.Thursday)
                .NotNull().WithMessage("Thursday is required.");

            RuleFor(x => x.Friday)
                .NotNull().WithMessage("Friday is required.");

            RuleFor(x => x.Saturday)
                .NotNull().WithMessage("Saturday is required.");

            RuleFor(x => x.Sunday)
                .NotNull().WithMessage("Sunday is required.");
        }
    }
}
