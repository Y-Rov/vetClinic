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
        public async Task GetAsync_AllUsers_ReturnsOkObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetAllUsersAsync())
                .ReturnsAsync(_fixture.Users);

            _fixture.MockReadEnumerableMapper
                .Setup(m => m.Map(It.IsAny<IEnumerable<User>>()))
                .Returns(_fixture.ReadViewModels);

            // Act
            var result = await _fixture.MockUserController.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _fixture.ReadViewModels);
        }

        [Fact]
        public async Task GetAsync_ExistingUser_ReturnsOkObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            _fixture.MockReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_fixture.ReadViewModel);

            // Act
            var result = await _fixture.MockUserController.GetAsync(_fixture.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _fixture.ReadViewModel);
        }

        [Fact]
        public async Task GetDoctorsAsync_AllDoctors_ReturnsOkObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetDoctorsAsync(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Users);

            _fixture.MockReadEnumerableMapper
                .Setup(m => m.Map(It.IsAny<IEnumerable<User>>()))
                .Returns(_fixture.ReadViewModels);

            // Act
            var result = await _fixture.MockUserController.GetDoctorsAsync(string.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as OkObjectResult)!.Value, _fixture.ReadViewModels);
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsCreatedAtActionObjectResult()
        {
            // Arrange
            _fixture.MockCreateMapper
                .Setup(m => m.Map(It.IsAny<UserCreateViewModel>()))
                .Returns(_fixture.User);

            _fixture.MockReadMapper
                .Setup(m => m.Map(It.IsAny<User>()))
                .Returns(_fixture.ReadViewModel);

            // Act
            var result = await _fixture.MockUserController.CreateAsync(_fixture.CreateViewModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((result.Result as CreatedAtActionResult)!.Value, _fixture.ReadViewModel);
        }

        [Fact]
        public async Task UpdateAsync_ExistingUser_ReturnsNoContentObjectResult()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            _fixture.MockUpdateMapper
                .Setup(m => m.Map(It.IsAny<UserUpdateViewModel>()))
                .Returns(_fixture.User);

            // Act
            var result = await _fixture.MockUserController.UpdateAsync(_fixture.Id, _fixture.UpdateViewModel);

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
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUserController.DeleteAsync(_fixture.Id);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserService.Verify();
        }
    }
}
