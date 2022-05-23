using Core.ViewModel;
using FluentValidation;

namespace WebApi.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        public UserCreateDtoValidator()
        {
            Include(new UserUpdateDtoValidator());

            RuleFor(u => u.Password)
                .Equal(u => u.ConfirmPassword)
                .WithMessage("Passwords do not match");
        }
    }
}
