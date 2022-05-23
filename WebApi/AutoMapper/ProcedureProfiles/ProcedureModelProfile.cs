using AutoMapper;
using Core.Entities;
using Core.Models;

namespace WebApi.AutoMapper.ProcedureProfiles;

public class ProcedureModelProfile : Profile
{
    public ProcedureModelProfile()
    {
        CreateMap<Procedure, ProcedureModel>();
    }
}