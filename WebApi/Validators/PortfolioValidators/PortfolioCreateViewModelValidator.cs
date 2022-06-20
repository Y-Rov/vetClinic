using Core.ViewModels.PortfolioViewModels;
using FluentValidation;

namespace WebApi.Validators.PortfolioValidators
{
    public class PortfolioCreateViewModelValidator : PortfolioBaseViewModelValidator<PortfolioCreateViewModel>
    {
        public PortfolioCreateViewModelValidator()
        {
            RuleFor(viewModel => viewModel.Id)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0");
        }
    }
}
