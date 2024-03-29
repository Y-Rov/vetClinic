﻿using Core.ViewModels;
using FluentValidation;

namespace WebApi.Validators
{
    public class AppointmentViewModelValidator : AbstractValidator<AppointmentReadViewModel>
    {
        public AppointmentViewModelValidator()
        {
            RuleFor(view => view.Date)
                .NotEmpty()
                .NotNull()
                .Must(IsNotLessNow)
                .WithMessage("Date must be not empty, not null and not less now");

            RuleFor(view => view.Disease)
                .NotEmpty()
                .NotNull()
                .WithMessage("Disease must be not empty, not null")
                .MinimumLength(3)
                .WithMessage("Disease must be more than 3 characters long")
                .MaximumLength(255)
                .WithMessage("Disease must be less than 255 characters long");
        }

        private bool IsNotLessNow(DateTime date)
        { 
            var result = (DateTime.Now.AddMinutes(30).CompareTo(date)) == -1;

                return result;
        }
    }
}
