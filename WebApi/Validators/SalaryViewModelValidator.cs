using Core.ViewModels.SalaryViewModel;
using FluentValidation;

namespace WebApi.Validators
{
    public class SalaryViewModelValidator: AbstractValidator<SalaryViewModel>
    {
        public SalaryViewModelValidator()
        {
            RuleFor(dto => dto.Value)
                .GreaterThan(0)
                .WithMessage("Salary value must be more than 0");
        }
    }
}
