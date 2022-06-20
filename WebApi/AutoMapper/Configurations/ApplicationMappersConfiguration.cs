using Core.Entities;
using Core.ViewModel;
using Core.ViewModels;
using Core.ViewModels.AddressViewModels;
using Core.ViewModels.AnimalViewModel;
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

        services.AddScoped<IViewModelMapper<Portfolio, PortfolioBaseViewModel>, PortfolioViewModelMapper>();
        services.AddScoped<IViewModelMapper<Portfolio, PortfolioCreateViewModel>, PortfolioCreateViewModelMapper>();
        services.AddScoped<IViewModelMapper<PortfolioCreateViewModel, Portfolio>, PortfolioCreateMapper>();
        services.AddScoped<IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio>, PortfolioUpdateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateViewModel>>, PortfolioReadEnumerableViewModelMapper>();

        services.AddScoped<IViewModelMapper<Address, AddressBaseViewModel>, AddressViewModelMapper>();
        services.AddScoped<IViewModelMapper<Address, AddressCreateViewModel>, AddressCreateViewModelMapper>();
        services.AddScoped<IViewModelMapper<AddressCreateViewModel, Address>, AddressCreateMapper>();
        services.AddScoped<IViewModelMapperUpdater<AddressBaseViewModel, Address>, AddressUpdateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateViewModel>>, AddressReadEnumerableViewModelMapper>();

        services.AddScoped<IViewModelMapper<Appointment, AppointmentViewModel>, AppointmentMapper>();
        services.AddScoped<IViewModelMapper<AppointmentViewModel, Appointment>, AppointmentViewModelMapper>();
        services.AddScoped <IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentViewModel>>, AppointmentsMapper>();
    }
}