using System.Text.RegularExpressions;
using Core.ViewModels.AddressViewModels;
using FluentValidation;

namespace WebApi.Validators.AddressValidators
{
    public class AddressCreateReadViewModelValidator<T> : AbstractValidator<T> where T : AddressCreateReadViewModel
    {
        public AddressCreateReadViewModelValidator()
        {
            RuleFor(viewModel => viewModel.City)
                .NotEmpty()
                .WithMessage("City name must be non-empty")
                .MaximumLength(85)
                .WithMessage("City name must be less than or equal to 85 symbols")
                .When(viewModel => typeof(AddressCreateReadViewModel) == viewModel.GetType());

            RuleFor(viewModel => viewModel.Street)
                .NotEmpty()
                .WithMessage("Street name must be non-empty")
                .MinimumLength(3)
                .WithMessage("Street name must be greater than or equal to 3 symbols")
                .MaximumLength(85)
                .WithMessage("Street name must be less than or equal to 85 symbols")
                .When(viewModel => typeof(AddressCreateReadViewModel) == viewModel.GetType());

            RuleFor(viewModel => viewModel.House)
                .NotEmpty()
                .WithMessage("House number description must be non-empty")
                .MaximumLength(15)
                .WithMessage("House number description length must be less than or equal to 15 symbols")
                .Matches(new Regex(@"^\d+(?: ?(?:[a-z]|[/-] ?\d+[a-z]?))?$", RegexOptions.IgnoreCase))
                .WithMessage("House number is not valid")
                .When(viewModel => typeof(AddressCreateReadViewModel) == viewModel.GetType());

            RuleFor(viewModel => viewModel.Id)
                .NotEmpty()
                .WithMessage("User ID must be non-empty")
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0");

            RuleFor(viewModel => viewModel.ApartmentNumber)
                .GreaterThan((ushort)0)
                .WithMessage("Apartment number must be greater than 0")
                .LessThanOrEqualTo((ushort)65535)
                .WithMessage("Apartment number is too big");

            RuleFor(viewModel => viewModel.ZipCode)
                .MinimumLength(5)
                .WithMessage("ZipCode must be greater than or equal to 5 symbols")
                .Matches(new Regex(@"^\d{5}(?:[-\s]\d{4})?$"))
                .WithMessage("ZipCode is not valid");
        }
    }
}

