using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using FluentAssertions;
using Moq;

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
                .ReturnsAsync(_fixture.TestAddresses);

            // Act
            var result = await _fixture.MockAddressService.GetAllAddressesAsync();

            // Assert
            Assert.NotNull(result);

            _fixture.MockLoggerManager
                .Verify(logger => logger.LogInfo("Getting all available addresses"), Times.Once);
            
            _fixture.TestAddresses
                .Should()
                .BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAddressByUserIdAsync_WhenAddressExists_ThenAddressIsReturned()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                   It.Is<int>(id => id >= 1 && id == _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.TestAddress);

            // Act
            var result = await _fixture.MockAddressService.GetAddressByUserIdAsync(_fixture.TestAddress.UserId);

            // Assert
            Assert.NotNull(result);
            
            _fixture.MockLoggerManager
                .Verify(logger => logger.LogInfo($"Address with UserID = {_fixture.TestAddress.UserId} was found"), Times.Once);
            
            _fixture.TestAddress
                .Should()
                .BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAddressByUserIdAsync_WhenAddressNotExists_ThenNotFoundExceptionIsThrown()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repository => repository.GetById(
                    It.Is<int>(id => id <= 0 || id != _fixture.TestAddress.UserId),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = _fixture.MockAddressService.GetAddressByUserIdAsync(_fixture.WrongUserId);

            // Assert
            _fixture.MockLoggerManager
                .Verify(logger => logger.LogWarn($"Address with UserID = {_fixture.WrongUserId} doesn't exist"), Times.Once);
            
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task CreateAddressAsync_WhenAddressIsNotExisting_ThenItIsSavedInDatabase()
        {
            // Arrange
            _fixture.MockAddressRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(id => id >= 1),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockUserService
                .Setup(service => service.GetUserByIdAsync(
                    It.Is<int>(id => id >= 1)))
                .ReturnsAsync(_fixture.TestUser);

            _fixture.MockAddressRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Address>()))
                .Returns(Task.CompletedTask).Verifiable();

            _fixture.MockAddressRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            // Act
            await _fixture.MockAddressService.CreateAddressAsync(_fixture.TestAddress);

            // Assert
            _fixture.MockAddressRepository
                .Verify(repo => repo.InsertAsync(It.IsAny<Address>()), Times.Once);

            _fixture.MockAddressRepository
                .Verify(repo => repo.SaveChangesAsync(), Times.Once);

            _fixture.MockLoggerManager
                .Verify(logger => logger.LogInfo($"Address for user with ID = {_fixture.TestAddress.UserId} was created"), Times.Once);
        }


    }
}
