using Core.ViewModels.ProcedureViewModels;
using FluentValidation;

namespace WebApi.Validators;

public class ProcedureViewModelBaseValidator : AbstractValidator<ProcedureViewModelBase>
{
    public ProcedureViewModelBaseValidator()
    {
        RuleFor(dto => dto.Cost)
            .GreaterThan(0)
            .WithMessage("Procedure cost must be greater than 0");
        RuleFor(dto => dto.Description)
            .MinimumLength(5)
            .WithMessage("Procedure description length must be greater than 5")
            .MaximumLength(250)
            .WithMessage("Procedure description length must be less than 250");
        RuleFor(dto => dto.Name)
            .MinimumLength(5)
            .WithMessage("Procedure name length must be greater than 5")
            .MaximumLength(100)
            .WithMessage("Procedure name length must be less than 250");
        RuleFor(dto => dto.DurationInMinutes)
            .GreaterThan(0)
            .WithMessage("Procedure duration must be greater than 0");
    }
}