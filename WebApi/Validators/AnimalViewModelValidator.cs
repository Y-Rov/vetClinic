using Core.ViewModels.AnimalViewModel;
using FluentValidation;

namespace WebApi.Validators
{
    public class AnimalViewModelValidator : AbstractValidator<AnimalViewModel>
    {
        public AnimalViewModelValidator()
        {
            RuleFor(dto => dto.NickName)
                .MinimumLength(5)
                .WithMessage("Animal nickname lenght must be greater that 5")
                .MaximumLength(100)
                .WithMessage("Animal nickname lenght must be less that 5");
        }
    }
}
