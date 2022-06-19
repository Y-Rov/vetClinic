using Core.Entities;
using Core.ViewModel;
using Core.ViewModels;
using Core.ViewModels.AddressViewModels;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.AppointmentsViewModel;
using Core.ViewModels.PortfolioViewModels;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SalaryViewModel;
using Core.ViewModels.SpecializationViewModels;
using Core.ViewModels.User;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper.AddressMappers;
using WebApi.AutoMapper.AnimalMappers;
using WebApi.AutoMapper.AppointmentMappers;
using WebApi.AutoMapper.ExceptionMapper;
using WebApi.AutoMapper.Interface;
using WebApi.AutoMapper.PortfolioMappers;
using WebApi.AutoMapper.ProcedureMappers;
using WebApi.AutoMapper.SalaryMappers;
using WebApi.AutoMapper.SpecializationMappers;
using WebApi.AutoMapper.UserMappers;


namespace WebApi.AutoMapper.Configurations;

public static class ApplicationMappersConfiguration
{
    // transfer to automapper
    public static void AddApplicationMappers(this IServiceCollection services)
    {
        services.AddScoped<IViewModelMapper<User, UserReadViewModel>, UserReadMapper>();
        services.AddScoped<IViewModelMapper<UserCreateViewModel, User>, UserCreateMapper>();
        services.AddScoped<IViewModelMapperUpdater<UserUpdateViewModel, User>, UserUpdateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>, UserReadEnumerableMapper>();

        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>, ExceptionsMapper>();

        services.AddScoped<IViewModelMapper<ProcedureUpdateViewModel, Procedure>, ProcedureUpdateMapper>();
        services.AddScoped<IViewModelMapper<ProcedureCreateViewModel, Procedure>, ProcedureCreateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>,
            ProcedureReadEnumerableMapper>();
        services.AddScoped<IViewModelMapper<Procedure, ProcedureReadViewModel>, ProcedureReadMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationViewModel>>,
                ProcedureSpecEnumerableMapper>();

        services.AddScoped<IViewModelMapper<Specialization, SpecializationViewModel>, SpecializationViewModelMapper>();
        services.AddScoped<IViewModelMapper<SpecializationViewModel, Specialization>, SpecializationMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>>, SpecializationListViewModelMapper>();

        services.AddScoped<IViewModelMapper<AnimalViewModel, Animal>, AnimalViewModelMapper>();
        services.AddScoped<IViewModelMapper<Animal, AnimalViewModel>, AnimalMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>, AnimalListToListMapper>();

        services.AddScoped<IViewModelMapper<Salary,SalaryViewModel>, SalaryMapper>();
        services.AddScoped<IViewModelMapper<SalaryViewModel, Salary>, SalaryViewModelMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>>, SalariesMapper>();

        services.AddScoped<IViewModelMapper<Portfolio, PortfolioViewModel>, PortfolioViewModelMapper>();
        services.AddScoped<IViewModelMapper<PortfolioViewModel, Portfolio>, PortfolioMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioViewModel>>,
            EnumerablePortfolioViewModelMapper>();

        services.AddScoped<IViewModelMapper<Address, AddressViewModel>, AddressViewModelMapper>();
        services.AddScoped<IViewModelMapper<AddressViewModel, Address>, AddressMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressViewModel>>,
            EnumerableAddressViewModelMapper>();
        
        services.AddScoped<IViewModelMapper<Appointment, AppointmentReadViewModel>, AppointmentReadMapper>();
        services.AddScoped<IViewModelMapper<AppointmentCreateViewModel, Appointment>, AppointmentCreateMapper>();
        services.AddScoped<IViewModelMapper<AppointmentUpdateViewModel, Appointment>, AppointmentUpdateModelMapper>();
        services.AddScoped <IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>, AppointmentReadEnumerableMapper>();
    }
}