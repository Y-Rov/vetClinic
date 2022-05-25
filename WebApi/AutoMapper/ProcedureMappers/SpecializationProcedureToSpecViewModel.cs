using Core.Entities;
using Core.Interfaces.Repositories;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class SpecializationProcedureToSpecViewModel :
    IViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>
{
    private readonly IViewModelMapper<Specialization, SpecializationViewModel> _specMapper;

    public SpecializationProcedureToSpecViewModel(IViewModelMapper<Specialization, SpecializationViewModel> specMapper)
    {
        _specMapper = specMapper;
    }

    public IEnumerable<SpecializationViewModel> Map(IEnumerable<ProcedureSpecialization> source)
    {
        var viewModels = new List<SpecializationViewModel>();
        foreach (var ps in source)
        {
            viewModels.Add(_specMapper.Map(ps.Specialization) );
        }

        return viewModels;
    }
}