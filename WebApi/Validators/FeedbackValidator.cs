using Core.Entities;
using Core.ViewModels.FeedbackViewModels;
using FluentValidation;

namespace WebApi.Validators
{
    public class FeedbackValidator : AbstractValidator<FeedbackCreateViewModel>
    {
        public FeedbackValidator()
        {
            RuleFor(feedback => feedback.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is incorrect");

            RuleFor(feedback => feedback.ServiceRate)
                .NotEmpty()
                .ExclusiveBetween(0, 5)
                .WithMessage("Service rate must be in range from 1 to 4");

            RuleFor(feedback => feedback.PriceRate)
                .NotEmpty()
                .ExclusiveBetween(0, 5)
                .WithMessage("Price rate must be in range from 1 to 4");

            RuleFor(feedback => feedback.SupportRate)
                .NotEmpty()
                .ExclusiveBetween(0, 5)
                .WithMessage("Support rate must be in range from 1 to 4");

            RuleFor(feedback => feedback.Suggestions)
                .MaximumLength(500)
                .WithMessage("Lenght must be less than 1000");
        }
    }
}
