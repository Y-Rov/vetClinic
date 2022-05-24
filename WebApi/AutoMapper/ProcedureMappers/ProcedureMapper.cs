using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureMapper : IViewModelMapper<ProcedureViewModelBase, Procedure>
{
    public Procedure Map(ProcedureViewModelBase source)
    {
        return new Procedure()
        {
            Id = source.Id,
            Name = source.Name,
            Cost = source.Cost,
            Description = source.Description,
            DurationInMinutes = source.DurationInMinutes
        };
    }
}