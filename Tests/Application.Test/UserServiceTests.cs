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
        private static readonly int _id = 1;
        private static readonly string _password = "test_pass";
        private static readonly string _role = "Client";

        private static readonly User _user = new()
        {
            Id = _id,
            FirstName = "Ren",
            LastName = "Amamiya"
        };

        private static readonly List<User> _users = new() { _user };

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
                .ReturnsAsync(_users);

            // Act
            var result = await _fixture.MockUserService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _users);
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_user);

            // Act
            var result = await _fixture.MockUserService.GetUserByIdAsync(_id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _user);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonexistingUser_ThrowsNotFoundException()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = _fixture.MockUserService.GetUserByIdAsync(_id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetDoctorsAsync_ValidData_ReturnsIEnumerableOfUser()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByRoleAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_users);

            // Act
            var result = await _fixture.MockUserService.GetDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, _users);
        }

        [Fact]
        public void CreateAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.CreateAsync(_user, _password);

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
            var result = _fixture.MockUserService.CreateAsync(_user, _password);

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
            var result = _fixture.MockUserService.AssignRoleAsync(_user, _role);

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
            var result = _fixture.MockUserService.AssignRoleAsync(_user, _role);

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
            var result = _fixture.MockUserService.UpdateAsync(_user);

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
            var result = _fixture.MockUserService.UpdateAsync(_user);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void DeleteAsync_ValidData_ReturnsVoid()
        {
            // Act
            var result = _fixture.MockUserService.DeleteAsync(_user);

            // Assert
            Assert.NotNull(result);
        }
    }
}
