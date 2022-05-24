using AutoMapper;
using Core.Entities;
using Core.ViewModels.ProcedureViewModels;

namespace WebApi.AutoMapper.ProcedureProfiles;

public class ProcedureDtoProfile : Profile
{
    public ProcedureDtoProfile()
    {
        CreateMap<ProcedureViewModel, Procedure>();
    }
}