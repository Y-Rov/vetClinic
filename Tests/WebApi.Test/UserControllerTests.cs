using Core.Entities;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class UserControllerTests : IClassFixture<UserControllerFixture>
    {
        private readonly UserControllerFixture _fixture;
        private static readonly int _id = 1;

        private static readonly User _user = new()
        {
            Id = _id,
            FirstName = "Ren",
            LastName = "Amamiya"
        };

        private static readonly UserReadViewModel _readViewModel = new()
        {
            Id = _id,
            FirstName = "Ren",
            LastName = "Amamiya"
        };

        private static readonly List<User> _users = new() { _user };
        private static readonly List<UserReadViewModel> _readViewModels = new() { _readViewModel };

        public UserControllerTests(UserControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_AllUsers_ReturnsOkObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(_users);

            _fixture.MockReadEnumerableMapper
                .Setup(m => m.Map(It.IsAny<IEnumerable<User>>()))
                .Returns(_readViewModels);

            // Act
            var result = await _fixture.MockUserController.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _readViewModels);
        }

        [Fact]
        public async Task GetAsync_ExistingUser_ReturnsOkObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_user);

            _fixture.MockReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_readViewModel);

            // Act
            var result = await _fixture.MockUserController.GetAsync(_id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _readViewModel);
        }

        [Fact]
        public async Task GetDoctorsAsync_AllDoctors_ReturnsOkObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetDoctorsAsync(It.IsAny<string>()))
                .ReturnsAsync(_users);

            _fixture.MockReadEnumerableMapper
                .Setup(m => m.Map(It.IsAny<IEnumerable<User>>()))
                .Returns(_readViewModels);

            // Act
            var result = await _fixture.MockUserController.GetDoctorsAsync(string.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _readViewModels);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsCreatedAtActionObjectResult()
        {
            // Arrange
            UserCreateViewModel createViewModel = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
                Password = "test_pass"
            };

            _fixture.MockCreateMapper
                .Setup(m => m.Map(It.IsAny<UserCreateViewModel>()))
                .Returns(_user);

            _fixture.MockReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_readViewModel);

            // Act
            var result = await _fixture.MockUserController.CreateAsync(createViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as CreatedAtActionResult)!.Value, _readViewModel);
        }

        [Fact]
        public async Task UpdateAsync_ExistingUser_ReturnsNoContentObjectResult()
        {
            // Arrange
            UserUpdateViewModel updateViewModel = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockUserService
                .Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_user);

            _fixture.MockUpdateMapper
                .Setup(m => m.Map(It.IsAny<UserUpdateViewModel>()))
                .Returns(_user);

            // Act
            var result = await _fixture.MockUserController.UpdateAsync(_id, updateViewModel);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserService.Verify();
        }

        [Fact]
        public async Task DeleteAsync_ExistingUser_ReturnsNoContentObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_user);

            // Act
            var result = await _fixture.MockUserController.DeleteAsync(_id);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserService.Verify();
        }
    }
}
