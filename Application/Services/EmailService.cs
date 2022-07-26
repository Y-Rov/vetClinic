using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        readonly string address;
        readonly string secret;
        readonly string protocol = "smtp.gmail.com";
        const int port = 587;

        IConfiguration _configuration;
        ILoggerManager _logger;
        IUserRepository _userRepository;

        public EmailService(
            IConfiguration configuration, 
            ILoggerManager logger,
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userRepository;
            address = _configuration.GetSection("Mailbox").GetSection("Address").Value;
            secret = _configuration.GetSection("Mailbox").GetSection("Secret").Value;
        }

        private async Task<List<string>?> GetRecipientsEmails(string role)
        {
            IEnumerable<User>? users = null;

            if (role == "employees")
                users = await _userRepository.GetByRolesAsync(
                    roleIds: new List<int> { 2, 3 });
            else if (role == "clients")
                users = await _userRepository.GetByRolesAsync(
                    roleIds: new List<int> { 4 });

            return users?.Select(user => user.Email).ToList();
        }


        public async Task NotifyUsers(Mailing message)
        {
            var usersEmails = await GetRecipientsEmails(message.Recipients);

            if(usersEmails == null)
            {
                _logger.LogError($"{message.Recipients} not found!");
                throw new NotFoundException($"{message.Recipients} not found!");
            }

            var createdMessage = new MimeMessage();
            var recipients = new List<MailboxAddress>();

            foreach (var recipient in usersEmails)
                recipients.Add(MailboxAddress.Parse(recipient));

            createdMessage.From.Add(new MailboxAddress("Vet clinic", address));
            createdMessage.To.AddRange(recipients);

            createdMessage.Subject = message.Subject;

            createdMessage.Body = new TextPart("html")
            {
                Text = message.Body
            };


            SmtpClient client = new SmtpClient();

            try
            {
                await client.ConnectAsync(protocol, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(address, "oxoelgyyqeyvyxzo");
                await client.SendAsync(createdMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error happened during sending email. Error: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            _logger.LogInfo("Email was successfully sended");
        }

        public async Task Send(EmailMessage message)
        {
            var createdMessage = new MimeMessage();
            createdMessage.From.Add(new MailboxAddress("Vet clinic", address));
            createdMessage.To.Add(MailboxAddress.Parse(message.Recipient));

            createdMessage.Subject = message.Subject;

            createdMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };
            SmtpClient client = new SmtpClient();

            try
            {
                await client.ConnectAsync(protocol, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(address, secret);
                await client.SendAsync(createdMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error happened during sending emails. Error: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            _logger.LogInfo("Emails were successfully sended");
        }
    }
}
