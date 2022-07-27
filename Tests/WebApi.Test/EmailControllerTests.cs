using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class EmailControllerTests : IClassFixture<EmailControllerFixture>, IDisposable
    {
        private readonly EmailControllerFixture _fixture;
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockService.ResetCalls();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public EmailControllerTests(EmailControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SendEmail_whenMessageIsValid_thenSendMessage()
        {
            await _fixture.MockController.SendEmail(_fixture.TestEmailMessage);

            _fixture.MockService
                .Verify(service => service.Send(_fixture.TestEmailMessage));
        }

        [Fact]
        public async Task NotifyUsers_whenMailingIsValid_thenNotify()
        {
            await _fixture.MockController.NotifyUsers(_fixture.TestMailing);

            _fixture.MockService
                .Verify(service => service.NotifyUsers(_fixture.TestMailing));
        }
    }
}
