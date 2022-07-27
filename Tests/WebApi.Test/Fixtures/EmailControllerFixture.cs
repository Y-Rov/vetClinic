using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Moq;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class EmailControllerFixture
    {
        public EmailControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockService = fixture.Freeze<Mock<IEmailService>>();

            MockController = new EmailController(MockService.Object);

            TestEmailMessage = GenerateMessage();
            TestMailing = GenerateMailing();
        }

        public EmailController MockController { get; }
        public Mock<IEmailService> MockService { get; }

        public EmailMessage TestEmailMessage { get; set; }
        public Mailing TestMailing { get; set; }

        private EmailMessage GenerateMessage()
        {
            return new EmailMessage()
            {
                Recipient = "test001@gmail.com",
                Subject = "Test message",
                Body = "Hello it's a test message"
            };
        }

        private Mailing GenerateMailing()
        {
            return new Mailing()
            {
                Recipients = "clients",
                Subject = "Discount",
                Body = "Hey, we have discount for you!"
            };
        }
    }
}
