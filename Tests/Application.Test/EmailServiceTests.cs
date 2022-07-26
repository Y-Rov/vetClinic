using Application.Test.Fixtures;
using Core.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using netDumbster.smtp;

namespace Application.Test
{
    public class EmailServiceTests : IClassFixture<EmailServiceFixture>, IDisposable
    {
        private SimpleSmtpServer _smtpServer;
        private readonly EmailServiceFixture _fixture;
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _smtpServer.Dispose();
                _fixture.MockUserRepository.ResetCalls();
                _fixture.MockLoggerManager.ResetCalls();
            }

            _disposed = true;
        }

        public EmailServiceTests(EmailServiceFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task SendEmail_whenMessageIsCorrect_thenSend()
        {

            _smtpServer = SimpleSmtpServer.Start(9000);

            await _fixture.MockService.Send(_fixture.TestEmailMessage);

            _fixture.MockLoggerManager.Verify(logger => logger.LogInfo("Email was successfully sended"), Times.Once);
        }
    }
}
