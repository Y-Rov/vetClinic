using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureSpecViewModelMapper : IViewModelMapper<Procedure, ProcedureSpecViewModel>
{
    private readonly IViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>> _internalMapper;

    public ProcedureSpecViewModelMapper(
        IViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>> internalMapper)
    {
        _internalMapper = internalMapper;
    }
    public ProcedureSpecViewModel Map(Procedure source)
    {
        var procedureSpecViewModel = new ProcedureSpecViewModel()
        {
            Id = source.Id,
            Name = source.Name,
            Cost = source.Cost,
            Description = source.Description,
            DurationInMinutes = source.DurationInMinutes
        };

        procedureSpecViewModel.Specializations =  _internalMapper.Map(source.ProcedureSpecializations);
        return procedureSpecViewModel;
    }
}