using Core.DTO.ProcedureDTOs;
using FluentValidation;

namespace WebApi.Validators;

public class ProcedureDtoValidator : AbstractValidator<ProcedureDTO>
{
    public ProcedureDtoValidator()
    {
        RuleFor(dto => dto.Cost)
            .GreaterThan(0);
        RuleFor(dto => dto.Description)
            .Length(10, 250);
        RuleFor(dto => dto.Name)
            .Length(10, 100);
        RuleFor(dto => dto.DurationInMinutes)
            .GreaterThan(0);
    }
}