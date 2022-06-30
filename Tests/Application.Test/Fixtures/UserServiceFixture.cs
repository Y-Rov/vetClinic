using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
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
            MockUserProfilePictureService = fixture.Freeze<Mock<IUserProfilePictureService>>();

            MockUserService = new UserService(
                MockUserRepository.Object,
                MockLoggerManager.Object,
                MockUserProfilePictureService.Object);

            Id = 1;
            Role = "Client";
            Passowrd = "test_pass";
            User = new() { Id = Id, FirstName = "Ren", LastName = "Amamiya" };
            Users = new List<User>() { User };
        }

        public UserService MockUserService { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public Mock<IUserProfilePictureService> MockUserProfilePictureService { get; set; }

        public int Id { get; set; }
        public string Role { get; set; }
        public string Passowrd { get; set; }
        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
