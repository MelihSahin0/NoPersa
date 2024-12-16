using FluentValidation;
using NoPersaService.Models;


namespace NoPersaService.FluentValidations
{
    public class DailyOverviewValidator : AbstractValidator<DailyOverview>
    {
        public DailyOverviewValidator() 
        {
            RuleFor(x => x.DayOfMonth)
                .NotNull().WithMessage("DayOfMonth is required.")
                .InclusiveBetween(1, 31).WithMessage("DayOfMonth must be between 1 and 31.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number or zero.");

            RuleFor(x => x.NumberOfBoxes)
               .GreaterThanOrEqualTo(0).WithMessage("Number Of Boxes must be a positive number or zero.");

            RuleFor(x => x.MonthlyOverviewId)
               .NotNull().WithMessage("MonthlyOverviewId is required.");
        }
    }
}
