using Core.ViewModels.User;
using FluentValidation;

namespace WebApi.Validators.User
{
    public class UserCreateValidator : UserBaseValidator<UserCreateViewModel>
    {
        public UserCreateValidator()
        {
            RuleFor(u => u.Password)
                .Equal(u => u.ConfirmPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
