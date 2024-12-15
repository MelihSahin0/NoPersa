using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator() 
        {
            RuleFor(x => x.SerialNumber)
                .MaximumLength(64).WithMessage("SerialNumber cannot be more than 64 characters.");

            RuleFor(x => x.Title)
                .MaximumLength(64).WithMessage("Title cannot be more than 64 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name cannot be more than 64 characters.");

            RuleFor(x => x.ContactInformation)
                .MaximumLength(64).WithMessage("ContactInformation cannot be more than 64 characters.");

            RuleFor(x => x.ArticleId)
                .NotNull().WithMessage("ArticleId is required.");

            RuleFor(x => x.DefaultNumberOfBoxes)
                .NotNull().WithMessage("Default Number Of Boxes is required.")
                .InclusiveBetween(0, 10).WithMessage("Default Number Of Boxes must be between 0 and 10.");
            
            RuleFor(x => x.Position)
                .NotNull().WithMessage("Position is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Position must be a positive number or zero.");

            RuleFor(x => x.TemporaryDelivery)
                .NotNull().WithMessage("TemporaryDelivery is required.");

            RuleFor(x => x.TemporaryNoDelivery)
                .NotNull().WithMessage("TemporaryNoDelivery is required.");

            RuleFor(x => x.WorkdaysId)
                .NotNull().WithMessage("WorkdaysId is required.");

            RuleFor(x => x.HolidaysId)
                .NotNull().WithMessage("HolidaysId is required.");

            RuleFor(x => x.RouteId)
                .NotNull().WithMessage("RouteId is required.");
        }
    }
}
