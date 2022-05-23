using AutoMapper;
using Core.ViewModel.ProcedureViewModels;
using Core.Entities;

namespace WebApi.AutoMapper.ProcedureProfiles;

public class ProcedureDtoProfile : Profile
{
    public ProcedureDtoProfile()
    {
        CreateMap<ProcedureViewModel, Procedure>();
    }
}