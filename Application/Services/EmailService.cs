using Core.Entities;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        public Task NotifyUsers(Mailing message)
        {
            throw new NotImplementedException();
        }

        public Task Send(EmailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
