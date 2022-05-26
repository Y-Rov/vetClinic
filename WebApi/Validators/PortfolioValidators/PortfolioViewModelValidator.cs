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
                .WithMessage("Portfolio description length must be greater than 63 symbols")
                .MaximumLength(2048)
                .WithMessage("Portfolio description length must be less than 2049 symbols");
        }
    }
}
