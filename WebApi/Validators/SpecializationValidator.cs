using Core.ViewModels.SpecializationViewModels;
using FluentValidation;

namespace WebApi.Validators
{
    public class SpecializationValidator : AbstractValidator<SpecializationViewModel>
    {
        public SpecializationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .WithMessage("Name must be assigned and less 50 characters");
        }
    }
}
