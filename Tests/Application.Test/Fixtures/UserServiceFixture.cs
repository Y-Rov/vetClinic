using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;

namespace Application.Test.Fixtures
{
    public class UserServiceFixture
    {
        public UserServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockUserService = fixture.Freeze<UserService>();
            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

            MockUserService = new UserService(
                MockUserRepository.Object,
                MockLoggerManager.Object);
        }

        public UserService MockUserService { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
    }
}
