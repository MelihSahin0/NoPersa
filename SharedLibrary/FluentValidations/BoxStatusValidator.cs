using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class BoxStatusValidator : AbstractValidator<BoxStatus>
    {
        public BoxStatusValidator()
        {
            RuleFor(x => x.NumberOfBoxesPreviousDay)
                .NotNull().WithMessage("NumberOfBoxesPreviousDay is required.")
                .InclusiveBetween(0, 10).WithMessage("Number of Boxes Previous Day must be between 0 and 10.");

            RuleFor(x => x.DeliveredBoxes)
                .NotNull().WithMessage("DeliveredBoxes is required.")
                .InclusiveBetween(0, 10).WithMessage("Delivered Boxes must be between 0 and 10.");

            RuleFor(x => x.ReceivedBoxes)
                .NotNull().WithMessage("ReceivedBoxes is required.")
                .InclusiveBetween(0, 10).WithMessage("Received Boxes must be between 0 and 10.");

            RuleFor(x => x.NumberOfBoxesCurrentDay)
                .NotNull().WithMessage("NumberOfBoxesCurrentDay is required.")
                .InclusiveBetween(0, 10).WithMessage("Number of Boxes Current Day must be between 0 and 10.");

            RuleFor(x => x.CustomerId)
                .NotNull().WithMessage("CustomerId is required.");
        }
    }
}
