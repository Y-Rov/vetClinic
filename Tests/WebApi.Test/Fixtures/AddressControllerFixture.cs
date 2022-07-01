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
            AddressWithApartmentNumber = new Address { UserId = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10 };
            AddressBaseViewModelWithApartmentNumber = new AddressBaseViewModel { City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10 };
            AddressCreateReadViewModelWithApartmentNumber = new AddressCreateReadViewModel { Id = UserId, City = "Lviv", Street = "Franka", House = "20A", ApartmentNumber = 10 };
            Addresses = new List<Address> { AddressWithApartmentNumber };
            AddressCreateReadViewModels = new List<AddressCreateReadViewModel> { AddressCreateReadViewModelWithApartmentNumber, };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAddressService = fixture.Freeze<Mock<IAddressService>>();
            MockAddressCreateMapper = fixture.Freeze<Mock<IViewModelMapper<AddressCreateReadViewModel, Address>>>();
            MockAddressReadViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<Address, AddressBaseViewModel>>>();
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
        }

        public AddressController MockAddressController { get; }
        public Mock<IAddressService> MockAddressService { get; }
        public Mock<IViewModelMapper<AddressCreateReadViewModel, Address>> MockAddressCreateMapper { get; }
        public Mock<IViewModelMapper<Address, AddressBaseViewModel>> MockAddressReadViewModelMapper { get; }
        public Mock<IViewModelMapperUpdater<AddressBaseViewModel, Address>> MockAddressUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Address>, IEnumerable<AddressCreateReadViewModel>>> MockAddressReadEnumerableViewModelMapper
        {
            get;
        }

        public int UserId { get; }
        public Address AddressWithApartmentNumber { get; set; }
        public AddressBaseViewModel AddressBaseViewModelWithApartmentNumber { get; set; }
        public AddressCreateReadViewModel AddressCreateReadViewModelWithApartmentNumber { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<AddressCreateReadViewModel> AddressCreateReadViewModels { get; set; }
    }
}