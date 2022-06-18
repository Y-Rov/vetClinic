using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureCreateMapper : IViewModelMapper<ProcedureCreateViewModel, Procedure>
{
    public Procedure Map(ProcedureCreateViewModel source)
    {
        return new Procedure()
        {
            Name = source.Name,
            Cost = source.Cost,
            Description = source.Description,
            DurationInMinutes = source.DurationInMinutes,
        };
    }
}