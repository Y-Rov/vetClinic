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
        public async Task GetAllUsersAsync_AllUsers_ReturnsIEnumerableOfUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>()))
                .ReturnsAsync(_fixture.Users);

            // Act
            var result = await _fixture.MockUserService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _fixture.Users);
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.User);

            // Act
            var result = await _fixture.MockUserService.GetUserByIdAsync(_fixture.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _fixture.User);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonexistingUser_ThrowsNotFoundException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            // Act
            var result = _fixture.MockUserService.GetUserByIdAsync(_fixture.Id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetDoctorsAsync_ValidData_ReturnsIEnumerableOfUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_fixture.Users);

            // Act
            var result = await _fixture.MockUserService.GetDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _fixture.Users);
        }

        [Fact]
        public void CreateAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Passowrd);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ThrowsBadRequestException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Passowrd);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void AssignRoleAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Role);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AssignRoleAsync_InvalidData_ThrowsBadRequestException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = _fixture.MockUserService.AssignRoleAsync(_fixture.User, _fixture.Role);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void UpdateAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.UpdateAsync(_fixture.User);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsBadRequestException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = _fixture.MockUserService.UpdateAsync(_fixture.User);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void DeleteAsync_ValidData_ReturnsVoid()
        {
            // Act
            var result = _fixture.MockUserService.DeleteAsync(_fixture.User);

            // Assert
            Assert.NotNull(result);
        }
    }
}
