using Core.Entities;
using Core.ViewModel;
using Core.ViewModels.AddressViewModels;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.PortfolioViewModels;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Core.ViewModels.User;
using Core.ViewModels.SalaryViewModel;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper.AddressMappers;
using WebApi.AutoMapper.AnimalMappers;
using WebApi.AutoMapper.ExceptionMapper;
using WebApi.AutoMapper.Interface;
using WebApi.AutoMapper.PortfolioMappers;
using WebApi.AutoMapper.ProcedureMappers;
using WebApi.AutoMapper.SpecializationMappers;
using WebApi.AutoMapper.UserMappers;
using WebApi.AutoMapper.SalaryMappers;

namespace WebApi.Configurations;

public static class ApplicationMappersConfiguration
{
    // transfer to automapper
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddScoped<IViewModelMapper<User, UserReadViewModel>, UserToUserReadViewModelMapper>();
        services.AddScoped<IViewModelMapper<UserCreateViewModel, User>, CreateViewModelToUserMapper>();
        services.AddScoped<IViewModelMapperUpdater<UserUpdateViewModel, User>, UpdateViewModelToUserMapper>();

        services.AddScoped<IViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>, ExceptionsMapper>();

        services.AddScoped<IViewModelMapper<ProcedureViewModelBase, Procedure>, ProcedureMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>,
            SpecializationProcedureToSpecViewModel>();
        services.AddScoped<IViewModelMapper<Procedure, ProcedureSpecViewModel>, ProcedureSpecViewModelMapper>();

        services.AddScoped<IViewModelMapper<Specialization, SpecializationViewModel>, SpecializationViewModelMapper>();
        services.AddScoped<IViewModelMapper<SpecializationViewModel, Specialization>, SpecializationMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>>, SpecializationListViewModel>();

        services.AddScoped<IViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>, ExceptionsMapper>();

        services.AddScoped<IViewModelMapper<AnimalViewModel, Animal>, AnimalViewModelMapper>();
        services.AddScoped<IViewModelMapper<Animal, AnimalViewModel>, AnimalMapper>();

        services.AddScoped<IViewModelMapper<Salary,SalaryViewModel>, SalaryMapper>();
        services.AddScoped<IViewModelMapper<SalaryViewModel, Salary>, SalaryViewModelMapper>();

        services.AddScoped<IViewModelMapper<Portfolio, PortfolioViewModel>, PortfolioViewModelMapper>();
        services.AddScoped<IViewModelMapper<PortfolioViewModel, Portfolio>, PortfolioMapper>();

        services.AddScoped<IViewModelMapper<Address, AddressViewModel>, AddressViewModelMapper>();
        services.AddScoped<IViewModelMapper<AddressViewModel, Address>, AddressModelMapper>();
    }
}