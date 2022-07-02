using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures
{
    public class AddressServiceFixture
    {
        public AddressServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAddressRepository = fixture.Freeze<Mock<IAddressRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

            MockAddressService = new AddressService(
                MockAddressRepository.Object,
                MockLoggerManager.Object);

            WrongUserId = 4;
            TestAddress = new Address { UserId = 1, City = "Ivano-Frankisk", Street = "Malanyuka", House = "14" };
            TestListOfAddresses = new List<Address> { TestAddress };
        }

        public IAddressService MockAddressService { get; }
        public Mock<IAddressRepository> MockAddressRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }

        public int WrongUserId { get; }
        public Address TestAddress { get; }
        public IList<Address> TestListOfAddresses { get; }
    }
}
