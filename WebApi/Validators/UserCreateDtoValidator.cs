using Core.ViewModels.User;
using FluentValidation;

namespace WebApi.Validators
{
    public class UserCreateValidator : UserValidator<UserCreateViewModel>
    {
        public UserCreateValidator()
        {
            RuleFor(u => u.Password)
                .Equal(u => u.ConfirmPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
