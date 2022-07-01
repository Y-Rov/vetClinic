using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
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

            Id = 1;
            Role = "Client";
            Passowrd = "test_pass";
            SpecializationName = "test_spec";
            Specialization = new() { Name = SpecializationName };

            UserSpecializations = new List<UserSpecialization>()
            {
                new UserSpecialization()
                {
                    Specialization = Specialization
                }
            };

            User = new() {
                Id = Id,
                FirstName = "Ren",
                LastName = "Amamiya",
                UserSpecializations = UserSpecializations
            };

            Users = new List<User>() { User };
        }

        public UserService MockUserService { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }

        public int Id { get; set; }
        public string Role { get; set; }
        public string Passowrd { get; set; }
        public string SpecializationName { get; set; }
        public Specialization Specialization { get; set; }
        public IEnumerable<UserSpecialization> UserSpecializations { get; set; }
        public User User { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
