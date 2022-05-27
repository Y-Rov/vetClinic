using Core.ViewModels.SalaryViewModel;
using FluentValidation;

namespace WebApi.Validators
{
    public class SalaryViewModelValidator: AbstractValidator<SalaryViewModel>
    {
        public SalaryViewModelValidator()
        {
            RuleFor(dto => dto.EmployeeId)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id must me min = 1");

            RuleFor(dto => dto.Value)
                .GreaterThan(0)
                .WithMessage("Salary value must be more than 0");
        }
    }
}
