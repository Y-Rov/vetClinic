using AutoMapper;
using Core.ViewModel.ProcedureDTOs;
using Core.Entities;

namespace WebApi.AutoMapper.ProcedureProfiles;

public class ProcedureDtoProfile : Profile
{
    public ProcedureDtoProfile()
    {
        CreateMap<ProcedureDTO, Procedure>();
    }
}