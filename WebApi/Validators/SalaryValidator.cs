using Core.ViewModels.SalaryViewModel;
using FluentValidation;

namespace WebApi.Validators
{
    public class SalaryValidator: AbstractValidator<SalaryViewModel>
    {
        public SalaryValidator()
        {
            RuleFor(dto => dto.Value)
                .GreaterThan(0)
                .WithMessage("Salary value must be more than 0");
        }
    }
}
