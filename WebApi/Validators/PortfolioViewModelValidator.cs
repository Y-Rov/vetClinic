using Core.ViewModels.PortfolioViewModels;
using FluentValidation;

namespace WebApi.Validators
{
    public class PortfolioViewModelValidator : AbstractValidator<PortfolioViewModel>
    {
        public PortfolioViewModelValidator()
        {
            RuleFor(viewModel => viewModel.UserId)
                .GreaterThan(0)
                .WithMessage("Portfolio user ID must be greater than 0!");

            RuleFor(viewModel => viewModel.Description)
                .MinimumLength(64)
                .WithMessage("Portfolio description length must be greater than 50 symbols!")
                .MaximumLength(2048)
                .WithMessage("Portfolio description length must be less than 1024 symbols!");
        }
    }
}
