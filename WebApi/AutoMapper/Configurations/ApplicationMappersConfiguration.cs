using Core.Entities;
using Core.Models;
using Core.Paginator;
using Core.Models.Finance;
using Core.ViewModels;
using Core.ViewModels.AddressViewModels;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.ArticleViewModels;
using Core.ViewModels.CommentViewModels;
using Core.ViewModels.ExceptionViewModel;
using Core.ViewModels.AppointmentsViewModel;
using Core.ViewModels.FeedbackViewModels;
using Core.ViewModels.PortfolioViewModels;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SalaryViewModel;
using Core.ViewModels.SpecializationViewModels;
using Core.ViewModels.User;
using Microsoft.Extensions.DependencyInjection;
using WebApi.AutoMapper.AddressMappers;
using WebApi.AutoMapper.AnimalMappers;
using WebApi.AutoMapper.AppointmentMappers;
using WebApi.AutoMapper.ArticleMappers;
using WebApi.AutoMapper.CommentMappers;
using WebApi.AutoMapper.ExceptionMapper;
using WebApi.AutoMapper.FeedbackMappers;
using WebApi.AutoMapper.ExceptionMappers;
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
        services.AddScoped<IViewModelMapper<PagedList<User>, PagedReadViewModel<UserReadViewModel>>, UserPagedMapper>();

        services.AddScoped<IViewModelMapper<ProcedureUpdateViewModel, Procedure>, ProcedureUpdateMapper>();
        services.AddScoped<IViewModelMapper<ProcedureCreateViewModel, Procedure>, ProcedureCreateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>,
            ProcedureReadEnumerableMapper>();
        services.AddScoped<IViewModelMapper<Procedure, ProcedureReadViewModel>, ProcedureReadMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<ProcedureSpecialization>, IEnumerable<SpecializationBaseViewModel>>,
                ProcedureSpecEnumerableMapper>();

        services.AddScoped<IViewModelMapper<Specialization, SpecializationViewModel>, SpecializationViewModelMapper>();
        services.AddScoped<IViewModelMapper<SpecializationViewModel, Specialization>, SpecializationMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>>, SpecializationListViewModelMapper>();

        services.AddScoped<IViewModelMapperUpdater<AnimalViewModel, Animal>, AnimalViewModelMapper>();
        services.AddScoped<IViewModelMapper<Animal, AnimalViewModel>, AnimalMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>, AnimalListToListMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AnimalMedCardViewModel>>, AnimalMedCardMapper>();
        services.AddScoped<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>>, AnimalPagedMedCardMapper>();

        services.AddScoped<IViewModelMapper<Salary, SalaryViewModel>, SalaryMapper>();
        services.AddScoped<IViewModelMapper<SalaryViewModel, Salary>, SalaryViewModelMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<FinancialStatement>,
            IEnumerable<FinancialStatementForMonthViewModel>>, FinancialStatementForMonthMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>>, SalariesMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>>, EmployeesMapper>();

        services.AddScoped<IViewModelMapper<Portfolio, PortfolioBaseViewModel>, PortfolioViewModelMapper>();
        services.AddScoped<IViewModelMapper<Portfolio, PortfolioCreateReadViewModel>, PortfolioCreateViewModelMapper>();
        services.AddScoped<IViewModelMapper<PortfolioCreateReadViewModel, Portfolio>, PortfolioCreateMapper>();
        services.AddScoped<IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio>, PortfolioUpdateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateReadViewModel>>, PortfolioReadEnumerableViewModelMapper>();

        services.AddScoped<IViewModelMapper<Address, AddressBaseViewModel>, AddressViewModelMapper>();
        services.AddScoped<IViewModelMapper<Address, AddressCreateReadViewModel>, AddressCreateViewModelMapper>();
        services.AddScoped<IViewModelMapper<AddressCreateReadViewModel, Address>, AddressCreateMapper>();
        services.AddScoped<IViewModelMapperUpdater<AddressBaseViewModel, Address>, AddressUpdateMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>, AddressReadEnumerableViewModelMapper>();

        services.AddScoped<IViewModelMapper<Article, ReadArticleViewModel>, ReadArticleMapper>();
        services.AddScoped<IViewModelMapper<CreateArticleViewModel, Article>, CreateArticleMapper>();
        services.AddScoped<IViewModelMapper<UpdateArticleViewModel, Article>, UpdateArticleMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>>, ReadEnumerableArticleMapper>();

        services.AddScoped<IViewModelMapper<Comment, ReadCommentViewModel>, ReadCommentMapper>();
        services.AddScoped<IViewModelMapper<CreateCommentViewModel, Comment>, CreateCommentMapper>();
        services.AddScoped<IViewModelMapper<UpdateCommentViewModel, Comment>, UpdateCommentMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>>, ReadEnumerableCommentMapper>();

        services.AddScoped<IViewModelMapper<PagedList<ExceptionEntity>, PagedReadViewModel<ExceptionEntityReadViewModel>>, PagedExceptionEntityMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>>, ExceptionsMapper>();
        services.AddScoped<IEnumerableViewModelMapper<IEnumerable<ExceptionStats>, IEnumerable<ExceptionStatsReadViewModel>>, ExceptionStatsMapper>();
        services.AddScoped<IViewModelMapper<PagedList<ExceptionStats>, PagedReadViewModel<ExceptionStatsReadViewModel>>, PagedExceptionStatsMapper>();
        services.AddScoped<IViewModelMapper<Appointment, AppointmentReadViewModel>, AppointmentReadMapper>();
        services.AddScoped<IViewModelMapper<AppointmentCreateViewModel, Appointment>, AppointmentCreateMapper>();
        services.AddScoped<IViewModelMapper<AppointmentUpdateViewModel, Appointment>, AppointmentUpdateModelMapper>();
        services.AddScoped <IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>, AppointmentReadEnumerableMapper>();

        services.AddScoped<IViewModelMapper<Feedback, FeedbackReadViewModel>, FeedbackMapper>();
        services.AddScoped<IViewModelMapper<FeedbackCreateViewModel, Feedback>, FeedbackCreateMapper>();
        services.AddScoped<IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>>, FeedbackViewModelListMapper>();
    }
}