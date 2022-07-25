using Core.Entities;
using Core.Interfaces;
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

        public EmailService(IConfiguration configuration, ILoggerManager logger)
        {
            _configuration = configuration;
            _logger = logger;
            address = _configuration.GetSection("Mailbox").GetSection("Address").Value;
            secret = _configuration.GetSection("Mailbox").GetSection("Secret").Value;
        }

        public async Task NotifyUsers(Mailing message)
        {
            var createdMessage = new MimeMessage();
            var recipients = new List<MailboxAddress>();

            foreach (var recipient in message.Recipients)
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
