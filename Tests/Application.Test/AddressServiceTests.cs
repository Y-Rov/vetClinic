using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class AddressServiceTests : IClassFixture<AddressServiceFixture>
    {
        private readonly AddressServiceFixture _fixture;

        public AddressServiceTests(AddressServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllAddressesAsync_WhenListOfAddressesIsRequested_ThenListOfAddressesIsReturned()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Address, bool>>>(),
                    It.IsAny<Func<IQueryable<Address>, IOrderedQueryable<Address>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.TestListOfAddresses);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = await _fixture.MockAddressService.GetAllAddressesAsync();

            // Assert
            _fixture.MockAddressRepository
                .Verify(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Address, bool>>>(),
                    It.IsAny<Func<IQueryable<Address>, IOrderedQueryable<Address>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Address>>(result);
        }

        [Fact]
        public async Task GetAddressByUserIdAsync_WhenAddressExists_ThenAddressIsReturned()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestAddress);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = await _fixture.MockAddressService.GetAddressByUserIdAsync(_fixture.TestAddress.UserId);

            // Assert
            _fixture.MockAddressRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            Assert.NotNull(result);
            Assert.IsType<Address>(result);
        }

        [Fact]
        public async Task GetAddressByUserIdAsync_WhenAddressNotExists_ThenNotFoundExceptionIsThrown()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(match => match != _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockAddressService.GetAddressByUserIdAsync(_fixture.WrongUserId);

            // Assert
            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            await Assert.ThrowsAsync<NotFoundException>(() => result);
            Assert.True(result.IsFaulted);
        }

        [Fact]
        public async Task CreateAddressAsync_WhenUserAddressWasUndefinedBefore_ThenItIsSavedInDatabaseAndTaskIsReturned()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockAddressRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Address>()))
                .Returns(Task.FromResult<object?>(null));

            _fixture.MockAddressRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null));

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockAddressService.CreateAddressAsync(_fixture.TestAddress);

            // Assert
            _fixture.MockAddressRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockAddressRepository
                .Verify(repo => repo.InsertAsync(It.IsAny<Address>()), Times.Once);

            _fixture.MockAddressRepository
                .Verify(repo => repo.SaveChangesAsync(), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            await result;
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task CreateAddressAsync_WhenUserAddressWasDefinedAlready_ThenBadRequestExceptionIsThrown()
        {
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestAddress);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockAddressService.CreateAddressAsync(_fixture.TestAddress);

            // Assert
            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            await Assert.ThrowsAsync<BadRequestException>(() => result);
            Assert.True(result.IsFaulted);
        }

        [Fact]
        public async Task UpdateAddressAsync_WhenAddressIsCorrect_ThenItIsUpdatedSuccessfullyAndTaskIsReturned()
        {
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestAddress);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockAddressService.UpdateAddressAsync(_fixture.TestAddress);

            // Assert
            _fixture.MockAddressRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            await result;
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task DeleteAddressByUserIdAsync_WhenUserIdIsCorrect_ThenAddressIsDeletedSuccessfullyAndTaskIsReturned()
        {
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestAddress);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogInfo(It.IsAny<string>()))
                .Verifiable();

            _fixture.MockAddressRepository
                .Setup(repo => repo.Delete(It.IsAny<Address>()));

            _fixture.MockAddressRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = _fixture.MockAddressService.DeleteAddressByUserIdAsync(_fixture.TestAddress.UserId);

            // Assert
            _fixture.MockAddressRepository
                .Verify(repository => repository.GetById(
                    It.Is<int>(userId => userId == _fixture.TestAddress.UserId),
                    It.IsAny<string>()), Times.Once);

            _fixture.MockAddressRepository
                .Verify(repo => repo.Delete(It.IsAny<Address>()), Times.Once);

            _fixture.MockAddressRepository
                .Verify(repo => repo.SaveChangesAsync(), Times.Once);

            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            await result;
            Assert.True(result.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task DeleteAddressByUserIdAsync_WhenUserIdIsIncorrect_ThenAddressIsDeletedSuccessfullyAndTaskIsReturned()
        {
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(userId => userId != _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockLoggerManager
                .Setup(logger => logger.LogWarn(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = _fixture.MockAddressService.DeleteAddressByUserIdAsync(_fixture.WrongUserId);

            // Assert
            _fixture.MockLoggerManager
                .Verify();

            _fixture.MockAddressRepository.ResetCalls();

            await Assert.ThrowsAsync<NotFoundException>(() => result);
            Assert.True(result.IsFaulted);
        }
    }
}
