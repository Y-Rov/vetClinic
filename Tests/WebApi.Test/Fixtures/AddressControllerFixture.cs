using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.AddressViewModels;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class AddressControllerFixture
    {
        public AddressControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAddressService = fixture.Freeze<Mock<IAddressService>>();
            MockAddressCreateMapper = fixture.Freeze<Mock<IViewModelMapper<AddressCreateReadViewModel, Address>>>();
            MockAddressReadViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<Address, AddressCreateReadViewModel>>>();
            MockAddressUpdateMapper = fixture.Freeze<Mock<IViewModelMapperUpdater<AddressBaseViewModel, Address>>>();
            MockAddressReadEnumerableViewModelMapper = fixture
                .Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>>>();

            MockAddressController = new AddressController(
                MockAddressService.Object,
                MockAddressCreateMapper.Object,
                MockAddressReadViewModelMapper.Object,
                MockAddressUpdateMapper.Object,
                MockAddressReadEnumerableViewModelMapper.Object
                );

            UserId = 1;
            FullAddress = GenerateAddress();
            FullAddressBaseViewModel = GenerateAddressBaseViewModel();
            FullAddressCreateReadViewModel = GenerateAddressCreateReadViewModel();
            Addresses = GenerateListOfAddresses();
            AddressCreateReadViewModels = GenerateListOfAddressCreateReadViewModels();
        }

        public AddressController MockAddressController { get; }
        public Mock<IAddressService> MockAddressService { get; }
        public Mock<IViewModelMapper<AddressCreateReadViewModel, Address>> MockAddressCreateMapper { get; }
        public Mock<IViewModelMapper<Address, AddressCreateReadViewModel>> MockAddressReadViewModelMapper { get; }
        public Mock<IViewModelMapperUpdater<AddressBaseViewModel, Address>> MockAddressUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>> MockAddressReadEnumerableViewModelMapper
        {
            get;
        }

        public int UserId { get; }
        public Address FullAddress { get; }
        public AddressBaseViewModel FullAddressBaseViewModel { get; }
        public AddressCreateReadViewModel FullAddressCreateReadViewModel { get; }
        public IEnumerable<Address> Addresses { get; }
        public IEnumerable<AddressCreateReadViewModel> AddressCreateReadViewModels { get; }

        private Address GenerateAddress()
        {
            var address = new Address { UserId = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10, ZipCode = "79007" };

            return address;
        }

        private AddressBaseViewModel GenerateAddressBaseViewModel()
        {
            var addressBaseViewModel = new AddressBaseViewModel { City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10, ZipCode = "79007" };

            return addressBaseViewModel;
        }

        private AddressCreateReadViewModel GenerateAddressCreateReadViewModel()
        {
            var addressCreateReadViewModel = new AddressCreateReadViewModel { Id = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10, ZipCode = "79007" };

            return addressCreateReadViewModel;
        }

        private IEnumerable<Address> GenerateListOfAddresses()
        {
            var addresses = new List<Address> { FullAddress };

            return addresses;
        }

        private IEnumerable<AddressCreateReadViewModel> GenerateListOfAddressCreateReadViewModels()
        {
            var addressCreateReadViewModels = new List<AddressCreateReadViewModel> { FullAddressCreateReadViewModel };

            return addressCreateReadViewModels;
        }
    }
}