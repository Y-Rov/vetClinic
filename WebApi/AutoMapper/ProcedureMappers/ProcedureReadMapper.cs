using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureReadMapper : IViewModelMapper<Procedure, ProcedureReadViewModel>
{
    private readonly IViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationBaseViewModel>> _internalMapper;

    public ProcedureReadMapper(
        IEnumerableViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationBaseViewModel>> internalMapper)
    {
        _internalMapper = internalMapper;
    }
    
    public ProcedureReadViewModel Map(Procedure source)
    {
        var procedureSpecViewModel = new ProcedureReadViewModel()
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