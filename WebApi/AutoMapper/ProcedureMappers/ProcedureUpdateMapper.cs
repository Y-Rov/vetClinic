using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureUpdateMapper : IViewModelMapper<ProcedureUpdateViewModel, Procedure>
{
    public Procedure Map(ProcedureUpdateViewModel source)
    {
        return new Procedure()
        {
            Id = source.Id,
            Name = source.Name,
            Cost = source.Cost,
            Description = source.Description,
            DurationInMinutes = source.DurationInMinutes,
        };
    }
}