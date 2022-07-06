using Core.ViewModels.SalaryViewModel;
using FluentValidation;


namespace WebApi.Validators.SalaryValidators
{
    public class DateViewModelValidator: AbstractValidator<DateViewModel>
    {
        public DateViewModelValidator()
        {
            RuleFor(dto => dto.EndDate)
                .NotEqual(start => start.StartDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("You try to get FinancialStatement for future");
        }
    }
}
