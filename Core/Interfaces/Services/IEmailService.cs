using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task Send(EmailMessage message);
        Task NotifyUsers(Mailing message);
    }
}
