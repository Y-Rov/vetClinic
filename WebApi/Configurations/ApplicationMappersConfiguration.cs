using Core.Entities;
using Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper.Interfaces;
using WebApi.AutoMapper.ProcedureMappers;

namespace WebApi.Configurations;

public static class ApplicationMappersConfiguration
{
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddSingleton<IViewModelMapper<ProcedureViewModelBase, Procedure>, ProcedureMapper>();
        services.AddSingleton<IViewModelMapper<Procedure, ProcedureWithSpecializationsViewModel>, 
            ProcedureWithSpecializationsViewModelMapper>();
    }
}