using Core.ViewModels.User;
using FluentValidation;
using System.Text.RegularExpressions;

namespace WebApi.Validators
{
    public class UserValidator<T> : AbstractValidator<T> where T : UserBaseViewModel
    {
        public UserValidator()
        {
            RuleFor(dto => dto.FirstName)
                .MinimumLength(1)
                .WithMessage("First name must be at least 1 character long")
                .MaximumLength(50)
                .WithMessage("First name must be less than 50 characters");

            RuleFor(u => u.LastName)
                .MinimumLength(1)
                .WithMessage("Last name must be at least 1 character long")
                .MaximumLength(50)
                .WithMessage("Last name must be less than 50 characters");

            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Incorrect Email format");

            RuleFor(u => u.PhoneNumber)
                .Matches(new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"))
                .WithMessage("Incorrect phone number format");

            RuleFor(u => u.BirthDate)
                .Must(BeAValidDate)
                .WithMessage("You must be older 14");
        }

        private bool BeAValidDate(DateTime date)
        {
            if ((DateTime.Now.Year - date.Year) < 14)
            {
                return false;
            }

            return true;
        }
    }
}
