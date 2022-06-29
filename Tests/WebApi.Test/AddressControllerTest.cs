using Core.Entities;
using Core.ViewModels.AddressViewModels;
using FluentAssertions;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class AddressControllerTest : IClassFixture<AddressControllerFixture>
    {
        private readonly AddressControllerFixture _fixture;

        public AddressControllerTest(AddressControllerFixture fixture)
        {
            _fixture = fixture;
        }

        public static IEnumerable<object[]> AddressesTypes()
        {
            foreach (AddressControllerFixture.AddressType type in Enum.GetValues(typeof(AddressControllerFixture.AddressType)))
            {
                yield return new object[] { type };
            }
        }

        [Fact]
        public async Task GetAsync_WhenAddressesListIsRequested_ThenListOfAddressCreateReadViewModelReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.GetAllAddressesAsync())
                .ReturnsAsync(_fixture.Addresses);

            _fixture.MockAddressReadEnumerableViewModelMapper
                .Setup(mapper => mapper.Map(It.IsAny<IEnumerable<Address>>()))
                .Returns(_fixture.AddressCreateReadViewModels);

            // Act
            var result = await _fixture.MockController.GetAsync();

            // Assert
            Assert.NotNull(result);
            _fixture.AddressCreateReadViewModels.Should().BeEquivalentTo(result);
        }

        [Theory]    
        [MemberData(nameof(AddressesTypes))]
        public async Task GetAsync_WhenUserIdIsCorrect_ThenAddressBaseViewModelReturned(AddressControllerFixture.AddressType addressType)
        {
            // Arrange
            var (address, addressBaseViewModel) = _fixture.GetAddressAndAddressBaseViewModel(addressType);

            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.Is<int>(id => id >= 1 && id == address.UserId)))
                .ReturnsAsync(address);

            _fixture.MockAddressReadViewModelMapper
                .Setup(mapper => mapper.Map(It.Is<Address>(match => match == address)))
                .Returns(addressBaseViewModel);

            // Act
            var result = await _fixture.MockController.GetAsync(_fixture.UserId);

            // Assert
            Assert.NotNull(result);
            addressBaseViewModel.Should().BeEquivalentTo(result);
        }

        [Theory]
        [MemberData(nameof(AddressesTypes))]
        public async Task CreateAsync_WhenAddressCreateReadViewModelIsCorrect_ThenNoContentResultReturned(AddressControllerFixture.AddressType addressType)
        {
            // Arrange
            var (address, addressCreateReadViewModel) = _fixture.GetAddressAndAddressCreateReadViewModel(addressType);

            _fixture.MockAddressCreateMapper
                .Setup(mapper => mapper.Map(It.Is<AddressCreateReadViewModel>(match => match == addressCreateReadViewModel)))
                .Returns(address);

            _fixture.MockAddressService
                .Setup(service => service.CreateAddressAsync(It.Is<Address>(mappedAddress => mappedAddress == address)))
                .Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _fixture.MockController.CreateAsync(addressCreateReadViewModel);

            // Assert
            Assert.NotNull(result);
            _fixture.MockAddressService.Verify(s => s.CreateAddressAsync(address), Times.Once);
        }

        [Theory]
        [MemberData(nameof(AddressesTypes))]
        public async Task UpdateAsync_WhenAddressBaseViewModelIsCorrect_ThenNoContentResultReturned(AddressControllerFixture.AddressType addressType)
        {
            // Arrange
            var (address, addressBaseViewModel) = _fixture.GetAddressAndAddressBaseViewModel(addressType);

            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.Is<int>(id => id >= 1 && id == address.UserId)))
                .ReturnsAsync(address);

            _fixture.MockAddressUpdateMapper
                .Setup(mapper => mapper.Map(
                    It.Is<AddressBaseViewModel>(match => match == addressBaseViewModel),
                    It.Is<Address>(match => match == address)));

            // Act
            var result = await _fixture.MockController.UpdateAsync(_fixture.UserId, addressBaseViewModel);

            // Assert
            Assert.NotNull(result);
            _fixture.MockAddressService.Verify(s => s.UpdateAddressAsync(address), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenUserIdIsCorrect_ThenNoContentResultReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.DeleteAddressByUserIdAsync(It.Is<int>(id => id >= 1 && id == _fixture.UserId)))
                .Returns(Task.CompletedTask).Verifiable();

            // Act
            var result = await _fixture.MockController.DeleteAsync(_fixture.UserId);

            // Assert
            Assert.NotNull(result);
            _fixture.MockAddressService.Verify(s => s.DeleteAddressByUserIdAsync(_fixture.UserId), Times.Once);
        }
    }
}
