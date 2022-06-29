using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class UserServiceTests : IClassFixture<UserServiceFixture>
    {
        private readonly UserServiceFixture _fixture;

        public UserServiceTests(UserServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllUsersAsync_whenUsersListIsNotEmpty_thenIEnumerableOfUserReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.Users);

            // Act
            var result = await _fixture.MockUserService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<User>>(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_whenUserExists_thenUserReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUserService.GetUserByIdAsync(_fixture.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_whenUsersDoesNotExist_thenNotFoundExceptionThrown()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Throws<NotFoundException>();

            // Act
            var result = _fixture.MockUserService.GetUserByIdAsync(_fixture.Id);

            // Assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<NotFoundException>(async () => await result);
        }

        [Fact]
        public async Task GetDoctorsAsync_whenDataIsValid_thenIEnumerableOfUserReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByRoleAsync(
                    It.IsAny<string>(), 
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>?>(), 
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.Users);

            // Act
            var result = await _fixture.MockUserService.GetDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<User>>(result);
        }

        [Fact]
        public void CreateAsync_whenDataIsValid_thenTaskReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Passowrd);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserRepository.Verify(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task CreateAsync_whenDataIsInvalid_thenBadRequestExceptionThrown()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Throws<BadRequestException>();

            // Act
            var result = _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Passowrd);

            // Assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<BadRequestException>(async () => await result);
        }

        [Fact]
        public void AssignRoleAsync_whenDataIsValid_thenTaskReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Role);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserRepository.Verify(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task AssignRoleAsync_whenDataIsInvalid_thenBadRequestExceptionThrown()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Throws<BadRequestException>();

            // Act
            var result = _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Role);

            // Assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<BadRequestException>(async () => await result);
        }

        [Fact]
        public void UpdateAsync_whenDataIsValid_thenTaskReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.UpdateAsync(_fixture.User);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task UpdateAsync_whenDataIsInvalid_thenTaskReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .Throws<BadRequestException>();

            // Act
            var result = _fixture.MockUserService.UpdateAsync(_fixture.User);

            // Assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<BadRequestException>(async () => await result);
        }

        [Fact]
        public void DeleteAsync_whenDataIsValid_thenVoidReturned()
        {
            // Act
            var result = _fixture.MockUserService.DeleteAsync(_fixture.User);

            // Assert
            Assert.NotNull(result);
            _fixture.MockUserRepository.Verify(r => r.Delete(It.IsAny<User>()), Times.Once());
        }
    }
}
