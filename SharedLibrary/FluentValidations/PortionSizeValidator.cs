using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class PortionSizeValidator : AbstractValidator<PortionSize>
    {
        public PortionSizeValidator() 
        {
            RuleFor(x => x.Position)
                .NotNull().WithMessage("Position is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Position must be a positive number or zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name cannot be more than 64 characters.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("IsDefault is required.");
        }
    }
}
