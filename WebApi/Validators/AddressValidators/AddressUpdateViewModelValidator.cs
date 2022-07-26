using System.Text.RegularExpressions;
using Core.ViewModels.AddressViewModels;
using FluentValidation;

namespace WebApi.Validators.AddressValidators
{
    public class AddressUpdateViewModelValidator : AddressBaseViewModelValidator<AddressUpdateViewModel>
    {
        public AddressUpdateViewModelValidator()
        {
            RuleFor(viewModel => viewModel.City)
                .MaximumLength(85)
                .WithMessage("City name must be less than or equal to 85 symbols")
                .When(viewModel => typeof(AddressUpdateViewModel) == viewModel.GetType());

            RuleFor(viewModel => viewModel.Street)
                .MinimumLength(3)
                .WithMessage("Street name must be greater than or equal to 3 symbols")
                .MaximumLength(85)
                .WithMessage("Street name must be less than or equal to 85 symbols");

            RuleFor(viewModel => viewModel.House)
                .MaximumLength(15)
                .WithMessage("House number description length must be less than or equal to 15 symbols")
                .Matches(new Regex(@"^\d+(?: ?(?:[a-z]|[/-] ?\d+[a-z]?))?$", RegexOptions.IgnoreCase))
                .WithMessage("House number is not valid");
        }
    }
}
