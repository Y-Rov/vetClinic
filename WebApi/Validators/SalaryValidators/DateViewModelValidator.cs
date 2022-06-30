using Core.ViewModels.SalaryViewModel;
using FluentValidation;


namespace WebApi.Validators.SalaryValidators
{
    public class DateViewModelValidator: AbstractValidator<DateViewModel>
    {
        public DateViewModelValidator()
        {
            RuleFor(dto => dto.endDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("You try to get FinancialStatement for future");
        }
    }
}
