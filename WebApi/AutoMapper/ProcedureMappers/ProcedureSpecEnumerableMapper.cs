using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureSpecEnumerableMapper : 
    IEnumerableViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>
{
    private readonly IViewModelMapper<Specialization, SpecializationViewModel> _specMapper;

    public ProcedureSpecEnumerableMapper(IViewModelMapper<Specialization, SpecializationViewModel> specMapper)
    {
        _specMapper = specMapper;
    }
    public IEnumerable<SpecializationViewModel> Map(IEnumerable<ProcedureSpecialization> source)
    {
        var specViewModels = source.Select(spec => _specMapper.Map(spec.Specialization));
        return specViewModels;
    }
}