using FluentValidation;
using SharedLibrary.Models;

namespace SharedLibrary.FluentValidations
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Position)
                .NotNull().WithMessage("Position is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Position must be a positive number or zero.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(64).WithMessage("Name cannot be more than 64 characters.");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number.");

            RuleFor(x => x.NewName)
                .NotEmpty().WithMessage("New Name is required.")
                .MaximumLength(64).WithMessage("NewName cannot be more than 64 characters.");

            RuleFor(x => x.NewPrice)
                .NotNull().WithMessage("New Price is required.")
                .GreaterThanOrEqualTo(0).WithMessage("NewPrice must be a positive number.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("IsDefault is required.");
        }
    }
}
