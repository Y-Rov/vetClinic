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
            UserId = 1;
            InitBaseAddresses();
            InitBaseAddressesWithApartmentNumbers();
            InitBaseAddressesWithZipCodes();
            InitFullAddresses();

            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAddressService = fixture.Freeze<Mock<IAddressService>>();
            MockAddressCreateMapper = fixture.Freeze<Mock<IViewModelMapper<AddressCreateReadViewModel, Address>>>();
            MockAddressReadViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<Address, AddressBaseViewModel>>>();
            MockAddressUpdateMapper = fixture.Freeze<Mock<IViewModelMapperUpdater<AddressBaseViewModel, Address>>>();
            MockAddressReadEnumerableViewModelMapper = fixture
                .Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>>>();

            MockController = new AddressController(
                MockAddressService.Object,
                MockAddressCreateMapper.Object,
                MockAddressReadViewModelMapper.Object,
                MockAddressUpdateMapper.Object,
                MockAddressReadEnumerableViewModelMapper.Object
                );

            Addresses = new List<Address> { BaseAddress, FullAddress };
            AddressCreateReadViewModels = new List<AddressCreateReadViewModel> { BaseAddressCreateReadViewModel, FullAddressCreateReadViewModel };
        }

        public enum AddressType
        {
            Base,
            BaseWithApartment,
            BaseWithZipCode,
            Full
        }

        public (Address, AddressBaseViewModel) GetAddressAndAddressBaseViewModel(AddressType type)
        {
            (Address, AddressBaseViewModel) baseAddressWithBaseViewModelTuple;
            switch (type)
            {
                case AddressType.Base:
                    baseAddressWithBaseViewModelTuple = (BaseAddress, BaseAddressViewModel);
                    break;
                case AddressType.BaseWithApartment:
                    baseAddressWithBaseViewModelTuple = (BaseAddressWithApartmentNumber, BaseAddressViewModelWithApartmentNumber);
                    break; 
                case AddressType.BaseWithZipCode:
                    baseAddressWithBaseViewModelTuple = (BaseAddressWithZipCode, BaseAddressViewModelWithZipCode);
                    break; 
                case AddressType.Full:
                    baseAddressWithBaseViewModelTuple = (FullAddress, FullAddressViewModel);
                    break;
                default:
                    baseAddressWithBaseViewModelTuple = (BaseAddress, BaseAddressViewModel);
                    break;
            }

            return baseAddressWithBaseViewModelTuple;
        }

        public AddressController MockController { get; }
        public Mock<IAddressService> MockAddressService { get; }
        public Mock<IViewModelMapper<AddressCreateReadViewModel, Address>> MockAddressCreateMapper { get; }
        public Mock<IViewModelMapper<Address, AddressBaseViewModel>> MockAddressReadViewModelMapper { get; }
        public Mock<IViewModelMapperUpdater<AddressBaseViewModel, Address>> MockAddressUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>> MockAddressReadEnumerableViewModelMapper
        {
            get;
        }

        public int UserId { get; set; }
        public Address BaseAddress { get; set; } = null!;
        public Address BaseAddressWithApartmentNumber { get; set; } = null!;
        public Address BaseAddressWithZipCode { get; set; } = null!;
        public Address FullAddress { get; set; } = null!;
        public AddressBaseViewModel BaseAddressViewModel { get; set; } = null!;
        public AddressBaseViewModel BaseAddressViewModelWithApartmentNumber { get; set; } = null!;
        public AddressBaseViewModel BaseAddressViewModelWithZipCode { get; set; } = null!;
        public AddressBaseViewModel FullAddressViewModel { get; set; } = null!;
        public AddressCreateReadViewModel BaseAddressCreateReadViewModel { get; set; } = null!;
        public AddressCreateReadViewModel BaseAddressCreateReadViewModelWithApartmentNumber { get; set; } = null!;
        public AddressCreateReadViewModel BaseAddressCreateReadViewModelWithZipCode { get; set; } = null!;
        public AddressCreateReadViewModel FullAddressCreateReadViewModel { get; set; } = null!;
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<AddressCreateReadViewModel> AddressCreateReadViewModels { get; set; }

        private void InitBaseAddresses()
        {
            BaseAddress = new Address() { UserId = UserId, City = "Lviv", Street = "Franka", House = "20A" };

            BaseAddressViewModel = new AddressBaseViewModel() { City = "Lviv", Street = "Franka", House = "20A" };

            BaseAddressCreateReadViewModel = new AddressCreateReadViewModel() { Id = UserId, City = "Lviv", Street = "Franka", House = "20A" };
        }

        private void InitBaseAddressesWithApartmentNumbers()
        {
            BaseAddressWithApartmentNumber = new Address() { UserId = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10 };

            BaseAddressViewModelWithApartmentNumber = new AddressBaseViewModel() { City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10 };

            BaseAddressCreateReadViewModelWithApartmentNumber = new AddressCreateReadViewModel() { Id = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10 };
        }

        private void InitBaseAddressesWithZipCodes()
        {
            BaseAddressWithZipCode = new Address() { UserId = UserId, City = "Lviv", Street = "Franka", House = "20A", ZipCode = "79007" };

            BaseAddressViewModelWithZipCode = new AddressBaseViewModel() { City = "Lviv", Street = "Franka", House = "20A", ZipCode = "79007" };

            BaseAddressCreateReadViewModelWithZipCode = new AddressCreateReadViewModel() { Id = UserId, City = "Lviv", Street = "Franka", House = "20A", ZipCode = "79007" };
        }

        private void InitFullAddresses()
        {
            FullAddress = new Address() { UserId = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10, ZipCode = "79007" };

            FullAddressViewModel = new AddressBaseViewModel() { City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10, ZipCode = "79007" };

            FullAddressCreateReadViewModel = new AddressCreateReadViewModel() { Id = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10, ZipCode = "79007" };
        }

    }
}