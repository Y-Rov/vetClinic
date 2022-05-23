using AutoMapper;
using Core.Entities;
using Core.ViewModels;
using WebApi.AutoMapper.Interfaces;

namespace WebApi.AutoMapper.ProcedureMappers;

public class ProcedureMapper : IViewModelMapper<ProcedureViewModelBase, Procedure>
{
    private readonly IMapper _mapper;
    public ProcedureMapper()
    {
        var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProcedureViewModelBase, Procedure>());
        _mapper = mapper.CreateMapper();
    }

    public Procedure Map(ProcedureViewModelBase source)
    {
        return _mapper.Map<ProcedureViewModelBase, Procedure>(source);
    }
    
    
}