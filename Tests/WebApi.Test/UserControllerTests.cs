using Core.Entities;
using Core.Models;
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
        public async Task GetAsync_whenUsersListIsNotEmpty_thenOkObjectResultReturned()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetAllUsersAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_fixture.Users);

            _fixture.MockReadEnumerableMapper
                .Setup(m => m.Map(It.IsAny<IEnumerable<User>>()))
                .Returns(_fixture.ReadViewModels);

            // Act
            var result = await _fixture.MockUserController.GetAsync(_fixture.CollateParameters);
            var readViewModels = (result.Result as OkObjectResult)!.Value as IEnumerable<UserReadViewModel>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<UserReadViewModel>>>(result);
            Assert.NotEmpty(readViewModels);
        }

        [Fact]
        public async Task GetAsync_whenUserExists_thenOkObjectResultReturned()
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
            var readViewModel = (result.Result as OkObjectResult)!.Value as UserReadViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<UserReadViewModel>>(result);
            Assert.NotNull(readViewModel);
        }

        [Fact]
        public async Task GetDoctorsAsync_whenDoctorsListIsNotEmpty_thenOkObjectResultReturned()
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
            var readViewModels = (result.Result as OkObjectResult)!.Value as IEnumerable<UserReadViewModel>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<IEnumerable<UserReadViewModel>>>(result);
            Assert.NotEmpty(readViewModels);
        }

        [Fact]
        public async Task CreateAsync_whenDataIsValid_thenCreatedAtActionResultReturned()
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
            var readViewModel = (result.Result as CreatedAtActionResult)!.Value as UserReadViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<UserReadViewModel>>(result);
            Assert.NotNull(readViewModel);
        }

        [Fact]
        public async Task UpdateAsync_whenUserExists_thenNoContentResultReturned()
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
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_whenUserExists_thenNoContentResultReturned()
        {
            // Arrange
            _fixture.MockUserService
                .Setup(s => s.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUserController.DeleteAsync(_fixture.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
