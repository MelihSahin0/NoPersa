using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class MaintenanceValidator : AbstractValidator<Maintenance>
    {
        public MaintenanceValidator() 
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.");

            RuleFor(x => x.Date)
                .NotNull().WithMessage("Date is required.");
        }
    }
}
