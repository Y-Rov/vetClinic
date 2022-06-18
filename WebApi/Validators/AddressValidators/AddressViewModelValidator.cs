﻿using Core.ViewModels.AddressViewModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace WebApi.Validators.AddressValidators
{
    public class AddressViewModelValidator : AbstractValidator<AddressViewModel>
    {
        public AddressViewModelValidator()
        {
            RuleFor(viewModel => viewModel.Id)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0");

            RuleFor(viewModel => viewModel.City)
                .NotEmpty()
                .WithMessage("City name must be non-empty")
                .MaximumLength(85)
                .WithMessage("City name must be less than or equal to 85 symbols");

            RuleFor(viewModel => viewModel.ZipCode)
                .MinimumLength(5)
                .WithMessage("ZipCode must be greater than or equal to 5 symbols")
                .Matches(new Regex(@"^\d{5}(?:[-\s]\d{4})?$"))
                .WithMessage("ZipCode is not valid");

            RuleFor(viewModel => viewModel.Street)
                .MinimumLength(3)
                .WithMessage("Street name must be greater than or equal to 3 symbols")
                .MaximumLength(85)
                .WithMessage("Street name must be less than or equal to 85 symbols");

            RuleFor(viewModel => viewModel.House)
                .NotEmpty()
                .WithMessage("House description must be non-empty")
                .MaximumLength(15)
                .WithMessage("House description length must be less than or equal to 15 symbols");

            RuleFor(viewModel => viewModel.ApartmentNumber)
                .GreaterThan((ushort)0)
                .WithMessage("Apartment number must be greater than 0");
        }
    }
}