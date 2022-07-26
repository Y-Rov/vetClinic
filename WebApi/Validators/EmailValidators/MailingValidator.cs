using Core.Entities;
using FluentValidation;

namespace WebApi.Validators.EmailValidators
{
    public class MailingValidator : AbstractValidator<Mailing>
    {
        public MailingValidator()
        {
            RuleFor(mailing => mailing.Recipients)
                .NotNull()
                .NotEmpty()
                .WithMessage("Recipients are required");

            RuleFor(email => email.Subject)
                .NotEmpty()
                .WithMessage("Subject is required")
                .MinimumLength(4)
                .WithMessage("Lenght must be at least 4")
                .MaximumLength(100);

            RuleFor(email => email.Body)
                .NotEmpty()
                .WithMessage("Message is required")
                .MinimumLength(10)
                .WithMessage("Lenght must be at least 10");
        }
    }
}
