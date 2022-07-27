using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore.Query;
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

            _smtpServer = SimpleSmtpServer.Start(9000);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task SendEmail_whenMessageIsCorrect_thenSend()
        {
            await _fixture.MockService.Send(_fixture.TestEmailMessage);

            _fixture.MockLoggerManager.Verify(logger => logger.LogInfo("Email was successfully sended"), Times.Once);
        }

        [Fact]
        public async Task NotifyUsers_whenRecipientsExist_thenSendEmails()
        {
            _fixture.MockUserRepository.Setup(repository =>
                repository.GetByRolesAsync(
                    It.IsAny<List<int>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>?>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>?>()))
                .ReturnsAsync(_fixture.ExpectedUsers);

            await _fixture.MockService.NotifyUsers(_fixture.TestMailing);

            _fixture.MockLoggerManager.Verify(logger => logger.LogInfo("Emails were successfully sended"), Times.Once);
        }

        [Fact]
        public async Task NotifyUsers_whenRecipientsNotExist_thenSendEmails()
        {
            _fixture.MockUserRepository.Setup(repository =>
                repository.GetByRolesAsync(
                    It.IsAny<List<int>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>?>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>?>()))
                .ReturnsAsync(_fixture.EmptyUsers);


            var result = _fixture.MockService.NotifyUsers(_fixture.TestMailing);

            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }
    }
}
