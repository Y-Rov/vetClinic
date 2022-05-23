using Core.Entities;
using Core.ViewModel;
using Core.ViewModels;
using WebApi.AutoMapper.Interfaces;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureWithSpecializationsViewModelMapper : IViewModelMapper<Procedure, ProcedureWithSpecializationsViewModel>
{
    public ProcedureWithSpecializationsViewModel Map(Procedure source)
    {
        var result = new ProcedureWithSpecializationsViewModel();
        result.Id = source.Id;
        result.Name = source.Name;
        result.Description = source.Description;
        result.Cost = source.Cost;
        foreach (var procedureSpecialization in source.ProcedureSpecializations ?? new List<ProcedureSpecialization>())
        {
            result.Specializations.Add(new SpecializationViewModel()
            {
                Id = procedureSpecialization.SpecializationId,
                Name = procedureSpecialization.Specialization.Name
            });
        }

        return result;
    }
}