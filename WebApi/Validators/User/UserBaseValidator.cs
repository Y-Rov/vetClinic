using Core.ViewModels.User;
using FluentValidation;
using System.Text.RegularExpressions;

namespace WebApi.Validators.User
{
    public class UserBaseValidator<T> : AbstractValidator<T> where T : UserBaseViewModel
    {
        public UserBaseValidator()
        {
            RuleFor(vm => vm.FirstName)
                .MinimumLength(2)
                .WithMessage("First name must be at least 2 character long")
                .MaximumLength(50)
                .WithMessage("First name must be less than 50 characters");

            RuleFor(vm => vm.LastName)
                .MinimumLength(2)
                .WithMessage("Last name must be at least 2 character long")
                .MaximumLength(50)
                .WithMessage("Last name must be less than 50 characters");

            RuleFor(vm => vm.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Incorrect Email format");

            RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .MinimumLength(10)
                .WithMessage("Phone number length must be at least 10 characters long")
                .MaximumLength(20)
                .WithMessage("Phone number length must be less than 20 characters")
                .Matches(new Regex(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"))
                .WithMessage("Incorrect phone number format");

            RuleFor(vm => vm.BirthDate)
                .Must(BeOlderEighteen)
                .WithMessage("You must be older 18");
        }

        private bool BeOlderEighteen(DateTime date)
        {
            if (DateTime.Now.Year - date.Year < 18)
            {
                return false;
            }

            return true;
        }
    }
}
