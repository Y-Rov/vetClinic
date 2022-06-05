using Core.ViewModels.PortfolioViewModels;
using FluentValidation;

namespace WebApi.Validators.PortfolioValidators
{
    public class PortfolioViewModelValidator : AbstractValidator<PortfolioViewModel>
    {
        public PortfolioViewModelValidator()
        {
            RuleFor(viewModel => viewModel.UserId)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0");

            RuleFor(viewModel => viewModel.Description)
                .MinimumLength(64)
                .WithMessage("Portfolio description must be greater than or equal to 64 symbols")
                .MaximumLength(2048)
                .WithMessage("Portfolio description must be less than or equal to 2048 symbols");
        }
    }
}
