using Core.Entities;
using Core.ViewModels.AddressViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class AddressControllerTest : IClassFixture<AddressControllerFixture>, IDisposable
    {
        private readonly AddressControllerFixture _fixture;
        private bool _disposed;

        public AddressControllerTest(AddressControllerFixture fixture)
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
                _fixture.MockAddressService.ResetCalls();
            }

            _disposed = true;
        }

        [Fact]
        public async Task GetAsync_WhenListOfAddressesIsNotEmpty_ThenListOfAddressCreateReadViewModelsIsReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.GetAllAddressesAsync())
                .ReturnsAsync(_fixture.Addresses);

            _fixture.MockAddressReadEnumerableViewModelMapper
                .Setup(mapper => mapper.Map(It.Is<IEnumerable<Address>>(match => match.Any())))
                .Returns(_fixture.AddressCreateReadViewModels)
                .Verifiable();

            // Act
            var result = await _fixture.MockAddressController.GetAsync();

            // Assert
            _fixture.MockAddressService
                .Verify(service => service.GetAllAddressesAsync(), Times.Once);
            
            _fixture.MockAddressReadEnumerableViewModelMapper
                .Verify();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsAssignableFrom<IEnumerable<AddressCreateReadViewModel>>(result);
        }

        [Fact]
        public async Task GetAsync_WhenListOfAddressesIsEmpty_ThenEmptyListOfAddressCreateReadViewModelsIsReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.GetAllAddressesAsync())
                .ReturnsAsync(new List<Address>());

            _fixture.MockAddressReadEnumerableViewModelMapper
                .Setup(mapper => mapper.Map(It.Is<IEnumerable<Address>>(match => !match.Any())))
                .Returns(new List<AddressCreateReadViewModel>())
                .Verifiable();

            // Act
            var result = await _fixture.MockAddressController.GetAsync();

            // Assert
            _fixture.MockAddressService
                .Verify(service => service.GetAllAddressesAsync(), Times.Once);

            _fixture.MockAddressReadEnumerableViewModelMapper
                .Verify();

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsAssignableFrom<IEnumerable<AddressCreateReadViewModel>>(result);
        }

        [Fact]
        public async Task GetAsync_WhenUserIdIsCorrect_ThenAddressBaseViewModelIsReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)))
                .ReturnsAsync(_fixture.FullAddress);

            _fixture.MockAddressReadViewModelMapper
                .Setup(mapper => mapper.Map(It.IsAny<Address>()))
                .Returns(_fixture.FullAddressCreateReadViewModel)
                .Verifiable();

            // Act
            var result = await _fixture.MockAddressController.GetAsync(_fixture.UserId);

            // Assert
            _fixture.MockAddressService
                .Verify(service => service.GetAddressByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)), Times.Once);

            _fixture.MockAddressReadViewModelMapper
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<AddressCreateReadViewModel>(result);
        }

        [Fact]
        public async Task CreateAsync_WhenAddressCreateReadViewModelIsCorrect_ThenNoContentResultIsReturned()
        {
            // Arrange
            _fixture.MockAddressCreateMapper
                .Setup(mapper => mapper.Map(It.IsAny<AddressCreateReadViewModel>()))
                .Returns(_fixture.FullAddress)
                .Verifiable();

            _fixture.MockAddressService
                .Setup(service => service.CreateAddressAsync(It.IsAny<Address>()))
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = await _fixture.MockAddressController.CreateAsync(_fixture.FullAddressCreateReadViewModel);

            // Assert
            _fixture.MockAddressService
                .Verify(service => service.CreateAddressAsync(It.IsAny<Address>()), Times.Once);

            _fixture.MockAddressCreateMapper
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateAsync_WhenAddressBaseViewModelIsCorrect_ThenNoContentResultIsReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)))
                .ReturnsAsync(_fixture.FullAddress);

            _fixture.MockAddressUpdateMapper
                .Setup(mapper => mapper.Map(
                    It.IsAny<AddressBaseViewModel>(),
                    It.IsAny<Address>()));

            _fixture.MockAddressService
                .Setup(service => service.UpdateAddressAsync(It.IsAny<Address>()))
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = await _fixture.MockAddressController.UpdateAsync(_fixture.UserId, _fixture.FullAddressBaseViewModel);

            // Assert
            _fixture.MockAddressService
                .Verify(service => service.GetAddressByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)), Times.Once);

            _fixture.MockAddressService
                .Verify(service => service.UpdateAddressAsync(It.IsAny<Address>()), Times.Once);

            _fixture.MockAddressCreateMapper
                .Verify();

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserIdIsCorrect_ThenNoContentResultIsReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.DeleteAddressByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)))
                .Returns(Task.FromResult<object?>(null));

            // Act
            var result = await _fixture.MockAddressController.DeleteAsync(_fixture.UserId);

            // Assert
            _fixture.MockAddressService
                .Verify(service => service.DeleteAddressByUserIdAsync(It.Is<int>(userId => userId == _fixture.UserId)), Times.Once);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
