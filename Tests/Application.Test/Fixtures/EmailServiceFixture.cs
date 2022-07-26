using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Application.Test.Fixtures
{
    public class EmailServiceFixture
    {
        public EmailServiceFixture()
        {
            var fixture =
               new Fixture().Customize(new AutoMoqCustomization());

            ExpectedUsers = GenerateUsers();
            EmptyUsers = GenerateEmptyUsers();

            TestEmailMessage = GenerateMessage();

            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(GenerateConfiguration())
                .Build();

            MockService = new EmailService(
                config,
                MockLoggerManager.Object,
                MockUserRepository.Object);
        }
        public EmailService MockService { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public Mock<IUserRepository> MockUserRepository { get; }

        public IEnumerable<User> ExpectedUsers { get; set; }
        public IEnumerable<User> EmptyUsers { get; set; }
        public EmailMessage TestEmailMessage { get; set; }

        private IEnumerable<User> GenerateUsers()
        {
            return new List<User>
            {
                new User
                {
                    FirstName = "Karen",
                    LastName = "Errgghh",
                    Email = "kk220@gmail.com"
                },
                new User
                {
                    FirstName = "Kujo",
                    LastName = "Sasuw",
                    Email = "gH220@gmail.com"
                }
            };
        }

        private IEnumerable<User> GenerateEmptyUsers()
        {
            return new List<User>();
        }

        private EmailMessage GenerateMessage()
        {
            return new EmailMessage()
            {
                Recipient = "test001@gmail.com",
                Subject = "Test message",
                Body = "Hello it's a test message"
            };
        }
        
        private Dictionary<string, string> GenerateConfiguration()
        {
            return new Dictionary<string, string>()
            {
                {"Mailbox:Address","vetclinicmanager222@gmail.com"},
                { "Mailbox:Secret", "oxoelgyyqeyvyxzo" }
            };
        }
    }
}
