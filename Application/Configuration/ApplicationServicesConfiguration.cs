using Application.Services;
using Application.Services.GeneratePDF.AnimalMedCard_PDF;
using Application.Services.GeneratePDF;
using Application.Services.GeneratePDF.FinancialStatement_PDF;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models.Finance;
using Core.Paginator.Parameters;
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

            services.AddScoped<IPDfGenerator, PDF_Generator>();
            services.AddScoped<ICreateTableForPDF<FinancialStatement>, CreateTableForFinancialStatementPDF>();
            services.AddScoped<IGenerateFullPDF<FinancialStatementParameters>, FinancialStatementPDfGenerator>();

            services.AddScoped<ICreateTableForPDF<Appointment>, CreateTableForAnimalMedCardPDF>();
            services.AddScoped<IGenerateFullPDF<AnimalParameters>, AnimalMedCardPDFGenerator>();
        }
    }
}
