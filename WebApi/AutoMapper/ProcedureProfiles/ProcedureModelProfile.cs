using AutoMapper;
using Core.Entities;
using Core.Models;
using Core.ViewModels.ProcedureViewModels;

namespace WebApi.AutoMapper.ProcedureProfiles;

public class ProcedureModelProfile : Profile
{
    public ProcedureModelProfile()
    {
        CreateMap<Procedure, ProcedureViewModelBase>();
    }
}