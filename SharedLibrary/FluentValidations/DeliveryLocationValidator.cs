using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class DeliveryLocationValidator : AbstractValidator<DeliveryLocation>
    {
        public DeliveryLocationValidator() 
        {
            RuleFor(x => x.Address)
               .NotEmpty().WithMessage("Address is required.")
               .MaximumLength(64).WithMessage("Address cannot be more than 64 characters.");

            RuleFor(x => x.Region)
               .NotEmpty().WithMessage("Region is required.")
               .MaximumLength(64).WithMessage("Region cannot be more than 64 characters.");
        
            RuleFor(x => x.Latitude)
               .NotNull().WithMessage("Latitude is required.")
               .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
               .NotNull().WithMessage("Longitude is required.")
               .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            RuleFor(x => x.CustomerId)
               .NotNull().WithMessage("CustomerId is required.");
        }
    }
}
