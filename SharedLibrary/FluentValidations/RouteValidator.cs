using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class RouteValidator : AbstractValidator<Route>
    {
        public RouteValidator() 
        {
            RuleFor(x => x.Position)
                .NotNull().WithMessage("Position is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Position must be a positive number or zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name cannot be more than 64 characters.");

            RuleFor(x => x.IsDrivable)
                .NotNull().WithMessage("IsDrivable is required.");
        }
    }
}
