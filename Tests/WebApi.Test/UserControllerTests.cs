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

        public UserControllerTests(UserControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_AllUsers_ReturnsIEnumerableOfUserReadViewModel()
        {
            // Arrange
            var users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "Ren",
                    LastName = "Amamiya"
                }
            };

            var readViewModels = new List<UserReadViewModel>()
            {
                new UserReadViewModel()
                {
                    Id = 1,
                    FirstName = "Ren",
                    LastName = "Amamiya"
                }
            };

            _fixture.MockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);
            _fixture.MockReadEnumerableMapper.Setup(m => m.Map(It.IsAny<IEnumerable<User>>())).Returns(readViewModels);

            // Act
            var result = await _fixture.MockUserController.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, readViewModels);
        }

        [Fact]
        public async Task GetAsync_ExistingUser_ReturnsUserReadViewModel()
        {
            // Arrange
            int id = 1;

            User user = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            UserReadViewModel readViewModel = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _fixture.MockReadMapper.Setup(m => m.Map(It.IsAny<User>())).Returns(readViewModel);

            // Act
            var result = await _fixture.MockUserController.GetAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, readViewModel);
        }

        [Fact]
        public async Task GetAsync_NonexistingUser_ReturnsUserReadViewModel()
        {
            // Arrange
            int id = 1;

            User user = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            UserReadViewModel readViewModel = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _fixture.MockReadMapper.Setup(m => m.Map(It.IsAny<User>())).Returns((UserReadViewModel)null!);

            // Act
            var result = await _fixture.MockUserController.GetAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual((result.Result as OkObjectResult)!.Value, readViewModel);
        }

        [Fact]
        public async Task GetDoctorsAsync_AllDoctors_ReturnsIEnumerableOfUserReadViewModel()
        {
            // Arrange
            List<User> users = new()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "Ren",
                    LastName = "Amamiya"
                }
            };

            List<UserReadViewModel> readViewModels = new()
            {
                new UserReadViewModel()
                {
                    Id = 1,
                    FirstName = "Ren",
                    LastName = "Amamiya"
                }
            };

            _fixture.MockUserService.Setup(s => s.GetDoctorsAsync(It.IsAny<string>())).ReturnsAsync(users);
            _fixture.MockReadEnumerableMapper.Setup(m => m.Map(It.IsAny<IEnumerable<User>>())).Returns(readViewModels);

            // Act
            var result = await _fixture.MockUserController.GetDoctorsAsync(string.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, readViewModels);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsActionResultOfUserReadViewModel()
        {
            // Arrange
            int id = 1;

            UserCreateViewModel createViewModel = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
                Password = "test_pass"
            };

            User user = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            UserReadViewModel readViewModel = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockCreateMapper.Setup(m => m.Map(It.IsAny<UserCreateViewModel>())).Returns(user);
            _fixture.MockReadMapper.Setup(m => m.Map(It.IsAny<User>())).Returns(readViewModel);

            // Act
            var result = await _fixture.MockUserController.CreateAsync(createViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as CreatedAtActionResult)!.Value, readViewModel);
        }

        [Fact]
        public async Task UpdateAsync_ExistingUser_ReturnsActionResult()
        {
            // Arrange
            int id = 1;

            UserUpdateViewModel updateViewModel = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            User user = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _fixture.MockUpdateMapper.Setup(m => m.Map(It.IsAny<UserUpdateViewModel>())).Returns(user);

            // Act
            var result = await _fixture.MockUserController.UpdateAsync(id, updateViewModel);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserService.Verify();
        }

        [Fact]
        public async Task DeleteAsync_ExistingUser_ReturnsActionResult()
        {
            // Arrange
            int id = 1;

            User user = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockUserService.Setup(s => s.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await _fixture.MockUserController.DeleteAsync(id);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserService.Verify();
        }
    }
}
