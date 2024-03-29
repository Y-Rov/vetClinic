﻿using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class UserServiceTests : IClassFixture<UserServiceFixture>, IDisposable
    {
        private readonly UserServiceFixture _fixture;
        private bool _disposed;

        public UserServiceTests(UserServiceFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockUserRepository.ResetCalls();
            }

            _disposed = true;
        }

        [Fact]
        public async Task GetAllUsersAsync_whenUsersListIsNotEmpty_thenIEnumerableOfUserReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetAllAsync(
                    It.IsAny<UserParameters>(),
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.PagedUsers);

            // Act
            var result = await _fixture.MockUserService.GetAllUsersAsync(_fixture.UserParameters);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<PagedList<User>>(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_whenUserExists_thenUserReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.GetByIdAsync(
                    It.IsAny<int>(), 
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
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
                .Setup(r => r.GetByIdAsync(
                    It.IsAny<int>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
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
                .Setup(r => r.GetByRolesAsync(
                    It.IsAny<List<int>>(),
                    It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(_fixture.Users);

            _fixture.MockUserRepository
                .Setup(r => r.FilterBySpecialization(It.IsAny<IEnumerable<User>>(), It.IsAny<string>()))
                .Returns(_fixture.Users);

            // Act
            var result = await _fixture.MockUserService.GetDoctorsAsync(_fixture.SpecializationName);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<List<User>>(result);
        }

        [Fact]
        public void CreateAsync_whenDataIsValid_thenTaskReturned()
        {
            // Arrange
            _fixture.MockUserRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Password);

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
            var result = _fixture.MockUserService.CreateAsync(_fixture.User, _fixture.Password);

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
            _fixture.MockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once());
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
