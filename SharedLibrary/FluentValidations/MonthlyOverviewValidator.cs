using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class MonthlyOverviewValidator : AbstractValidator<MonthlyOverview>
    {
        public MonthlyOverviewValidator() 
        {
            RuleFor(x => x.Year)
                .NotNull().WithMessage("Year is required.")
                .GreaterThanOrEqualTo(1900).WithMessage("Year must be a positive number greater then 1900");

            RuleFor(x => x.Month)
               .NotNull().WithMessage("Month is required.")
               .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.DailyOverviews)
                .Must(list => list.Count == 0 || list.Count >= 28)
                .WithMessage("The list must either be empty or have at least 28 items.");

            RuleFor(x => x.CustomerId)
               .NotNull().WithMessage("CustomerId is required.");
        }
    }
}
