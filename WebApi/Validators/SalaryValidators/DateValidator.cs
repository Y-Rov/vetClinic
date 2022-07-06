using Core.Models.Finance;
using FluentValidation;


namespace WebApi.Validators.SalaryValidators
{
    public class DateValidator: AbstractValidator<Date>
    {
        public DateValidator()
        {
            RuleFor(dto => dto.EndDate)
                .NotEqual(start => start.StartDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("You try to get FinancialStatement for future");
        }
    }
}
