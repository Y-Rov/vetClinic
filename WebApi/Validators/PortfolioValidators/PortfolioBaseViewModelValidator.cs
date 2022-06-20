using Core.ViewModels.PortfolioViewModels;
using FluentValidation;

namespace WebApi.Validators.PortfolioValidators
{
    public class PortfolioBaseViewModelValidator<T> : AbstractValidator<T> where T : PortfolioBaseViewModel
    {
        public PortfolioBaseViewModelValidator()
        {
            RuleFor(viewModel => viewModel.Description)
                .MinimumLength(64)
                .WithMessage("Portfolio description must be greater than or equal to 64 symbols")
                .MaximumLength(2048)
                .WithMessage("Portfolio description must be less than or equal to 2048 symbols");
        }
    }
}
