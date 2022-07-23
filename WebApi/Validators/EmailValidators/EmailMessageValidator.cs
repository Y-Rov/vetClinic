using Core.Entities;
using FluentValidation;

namespace WebApi.Validators.EmailValidators
{
    public class EmailMessageValidator : AbstractValidator<EmailMessage>
    {
        public EmailMessageValidator()
        {
            RuleFor(email => email.Recipient)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email");

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
                .WithMessage("Lenght must be at least 10")
                .MaximumLength(2400);
        }
    }
}
