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
            var users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "Ren",
                    LastName = "Amamiya"
                }
            };

            _fixture.MockUserRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>()))
                    .ReturnsAsync(users);

            // Act
            var result = await _fixture.MockUserService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, users);
        }

        [Fact]
        public async Task GetUserByIdAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            int id = 1;

            User user = new()
            {
                Id = id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };

            _fixture.MockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await _fixture.MockUserService.GetUserByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, user);
        }

        [Fact]
        public async Task GetUserByIdAsync_NonexistingUser_ThrowsNotFoundException()
        {
            // Arrange
            int id = 1;

            _fixture.MockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null!);

            // Act
            var result = _fixture.MockUserService.GetUserByIdAsync(id);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetDoctorsAsync_ValidData_ReturnsIEnumerableOfUser()
        {
            // Arrange
            int id = 1;

            var users = new List<User>()
            {
                new User()
                {
                    Id = id,
                    FirstName = "Ren",
                    LastName = "Amamiya"
                }
            };

            _fixture.MockUserRepository.Setup(r => r.GetByRoleAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(users);

            // Act
            var result = await _fixture.MockUserService.GetDoctorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, users);
        }

        [Fact]
        public void CreateAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            string password = "test_pass";

            _fixture.MockUserRepository.Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.CreateAsync(user, password);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_InvalidData_ThrowsBadRequestException()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            string password = "test_pass";

            _fixture.MockUserRepository.Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = _fixture.MockUserService.CreateAsync(user, password);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void AssignRoleAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            string role = "Client";

            _fixture.MockUserRepository.Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.AssignRoleAsync(user, role);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task AssignRoleAsync_InvalidData_ThrowsBadRequestException()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            string role = "Client";

            _fixture.MockUserRepository.Setup(r => r.AssignRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = _fixture.MockUserService.AssignRoleAsync(user, role);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void UpdateAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            _fixture.MockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.UpdateAsync(user);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsBadRequestException()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            _fixture.MockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            // Act
            var result = _fixture.MockUserService.UpdateAsync(user);

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => result);
        }

        [Fact]
        public void DeleteAsync_ValidData_ReturnsVoid()
        {
            // Arrange
            User user = new()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
            };

            // Act
            var result = _fixture.MockUserService.DeleteAsync(user);

            // Assert
            Assert.NotNull(result);
        }
    }
}
