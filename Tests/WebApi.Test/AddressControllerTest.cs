using Core.Entities;
using Core.ViewModels.AddressViewModels;
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

        [Theory]
        [MemberData(nameof(AddressesTypes))]
        public async Task GetAddressByUserId_WhenUserIdCorrect_ThenStatusCodeOKReturned(AddressControllerFixture.AddressType addressType)
        {
            // Arrange
            var (address, addressBaseViewModel) = _fixture.GetAddressAndAddressBaseViewModel(addressType);

            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.IsInRange(1, int.MaxValue, Moq.Range.Inclusive)))
                .ReturnsAsync(address);

            _fixture.MockAddressReadViewModelMapper
                .Setup(mapper => mapper.Map(It.IsAny<Address>()))
                .Returns(addressBaseViewModel);

            // Act
            var result = await _fixture.MockController.GetAsync(_fixture.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addressBaseViewModel, result);
        }

        [Fact]
        public async Task GetBaseAddressByUserId_WhenUserIdCorrect_ThenStatusCodeOKReturned()
        {
            // Arrange
            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.IsInRange(1, int.MaxValue, Moq.Range.Inclusive)))
                .ReturnsAsync(_fixture.BaseAddress);

            _fixture.MockAddressReadViewModelMapper
                .Setup(mapper => mapper.Map(It.IsAny<Address>()))
                .Returns(_fixture.BaseAddressViewModel);

            // Act
            var result = await _fixture.MockController.GetAsync(_fixture.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_fixture.BaseAddressViewModel, result);
        }
    }
}
