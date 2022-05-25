using Core.Entities;
using Core.Interfaces.Repositories;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class SpecializationProcedureToSpecViewModel :
    IViewModelMapperAsync<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>
{
    private readonly ISpecializationRepository _specializationRepository;

    public SpecializationProcedureToSpecViewModel(ISpecializationRepository specializationRepository)
    {
        _specializationRepository = specializationRepository;
    }

    public async Task<IEnumerable<SpecializationViewModel>> MapAsync(IEnumerable<ProcedureSpecialization> source)
    {
        var viewModels = new List<SpecializationViewModel>();
        foreach (var pr in source)
        {
            viewModels.Add(new SpecializationViewModel()
            {
                Id = pr.SpecializationId,
                Name = (await _specializationRepository.GetSpecializationByIdAsync(pr.SpecializationId)).Name
            });
        }

        return viewModels;
    }
}