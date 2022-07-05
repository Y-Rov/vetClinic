using Core.ViewModels.AddressViewModels;
using FluentValidation;

namespace WebApi.Validators.AddressValidators
{
    public class AddressCreateViewModelValidator : AddressBaseViewModelValidator<AddressCreateReadViewModel>
    {
        public AddressCreateViewModelValidator()
        {
            RuleFor(viewModel => viewModel.Id)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0");
        }
    }
}

