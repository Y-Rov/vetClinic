using Core.Entities;
using Core.ViewModel;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.User;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper.ExceptionMapper;
using WebApi.AutoMapper.AnimalMappers;
using WebApi.AutoMapper.Interface;
using WebApi.AutoMapper.ProcedureMappers;
using WebApi.AutoMapper.UserMappers;


namespace WebApi.Configurations;

public static class ApplicationMappersConfiguration
{
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddScoped<IViewModelMapper<User, UserReadViewModel>, UserToUserReadViewModelMapper>();
        services.AddScoped<IViewModelMapper<UserCreateViewModel, User>, CreateViewModelToUserMapper>();
        services.AddScoped<IViewModelMapperUpdater<UserUpdateViewModel, User>, UpdateViewModelToUserMapper>();

        services.AddScoped<IViewModelMapper<ProcedureViewModelBase, Procedure>, ProcedureMapper>();
        services.AddScoped<IViewModelMapperAsync<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>,
            SpecializationProcedureToSpecViewModel>();
        services.AddScoped<IViewModelMapperAsync<Procedure, ProcedureSpecViewModel>, ProcedureSpecViewModelMapper>();
        services.AddScoped<IViewModelMapper<AnimalViewModel, Animal>, AnimalViewModelMapper>();
        services.AddScoped<IViewModelMapper<Animal, AnimalViewModel>, AnimalMapper>();
    }
}