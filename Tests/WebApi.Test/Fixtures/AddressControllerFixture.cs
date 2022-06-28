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
    }
}