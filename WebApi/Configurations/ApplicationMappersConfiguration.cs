using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper.Interface;
using WebApi.AutoMapper.ProcedureMappers;
using WebApi.AutoMapper.SpecializationMappers;

namespace WebApi.Configurations;

public static class ApplicationMappersConfiguration
{
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddScoped<IViewModelMapper<ProcedureViewModelBase, Procedure>, ProcedureMapper>();
        services.AddScoped<IViewModelMapperAsync<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>,
            SpecializationProcedureToSpecViewModel>();
        services.AddScoped<IViewModelMapperAsync<Procedure, ProcedureSpecViewModel>, ProcedureSpecViewModelMapper>();
        services.AddScoped<IViewModelMapper<Specialization, SpecializationViewModel>, SpecializationViewModelMapper>();
        services.AddScoped<IViewModelMapper<SpecializationViewModel, Specialization>, SpecializationMapper>();
    }
}