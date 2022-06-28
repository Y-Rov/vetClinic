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

        [Fact]
        public async Task GetAddressByUserId_WhenUserIdCorrect_ThenStatusCodeOKReturned()
        {
            // Arrange
            int userId = 1;
            var address = new Address()
            {
                UserId = userId,
                City = "Lviv",
                Street = "Franka",
                House = "20A"
            };

            var addressBaseViewModel = new AddressBaseViewModel()
            {
                City = "Lviv",
                Street = "Franka",
                House = "20A"
            };

            _fixture.MockAddressService
                .Setup(service => service.GetAddressByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(address);

            _fixture.MockAddressReadViewModelMapper
                .Setup(mapper => mapper.Map(It.IsAny<Address>()))
                .Returns(addressBaseViewModel);

            // Act
            var result = await _fixture.MockController.GetAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(address.City, result.City);
        }
    }
}
