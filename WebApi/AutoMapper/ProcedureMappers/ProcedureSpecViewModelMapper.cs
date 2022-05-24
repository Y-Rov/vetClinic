using Core.Entities;
using Core.ViewModel;
using Core.ViewModels.ProcedureViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureSpecViewModelMapper : IViewModelMapperAsync<Procedure, ProcedureSpecViewModel>
{
    private readonly IViewModelMapperAsync<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>> _internalMapper;

    public ProcedureSpecViewModelMapper(
        IViewModelMapperAsync<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>> internalMapper)
    {
        _internalMapper = internalMapper;
    }
    public async Task<ProcedureSpecViewModel> MapAsync(Procedure source)
    {
        var procedureSpecViewModel = new ProcedureSpecViewModel()
        {
            Id = source.Id,
            Name = source.Name,
            Cost = source.Cost,
            Description = source.Description,
            DurationInMinutes = source.DurationInMinutes
        };

        procedureSpecViewModel.Specializations = await _internalMapper.MapAsync(source.ProcedureSpecializations);
        return procedureSpecViewModel;
    }
}