using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureSpecEnumerableMapper : 
    IEnumerableViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationBaseViewModel>>
{
    public IEnumerable<SpecializationBaseViewModel> Map(IEnumerable<ProcedureSpecialization> source)
    {
        var specViewModels = source.Select(spec => new SpecializationBaseViewModel()
        {
            Id = spec.SpecializationId,
            Name = spec.Specialization.Name
        });
        return specViewModels;
    }
}