using Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Repositories.Configuration
{
    public static class ApplicationRepositoriesConfiguration
    {
        public static void AddApplicationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IProcedureSpecializationRepository, ProcedureSpecializationRepository>();
            services.AddScoped<IProcedureRepository, ProcedureRepository>();
            services.AddScoped<IExceptionEntityRepository, ExceptionEntityRepository>();
            services.AddScoped<IAnimalRepository, AnimalRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUserSpecializationRepository, UserSpecializationRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUserProfilePictureRepository, UserProfilePictureRepository>();
            services.AddScoped<IAppointmentProcedureRepository, AppointmentProcedureRepository>();
            services.AddScoped<IAppointmentUserRepository, AppointmentUserRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        }
    }
}
