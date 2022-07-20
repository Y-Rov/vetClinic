using Application.Services;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configuration
{
    public static class ApplicationServicesConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAnimalService, AnimalService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IExceptionEntityService, ExceptionEntityService>();
            services.AddScoped<IProcedureService, ProcedureService>();
            services.AddScoped<IFinancialService, FinancialService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IUserProfilePictureService, UserProfilePictureService>();
            services.AddScoped<IImageParser, ImageParser>();
            services.AddScoped<IAnimalPhotoService, AnimalPhotoService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
        }
    }
}
