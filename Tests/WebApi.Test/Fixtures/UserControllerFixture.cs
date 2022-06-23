using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.User;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class UserControllerFixture
    {
        public UserControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockService = fixture.Freeze<Mock<IUserService>>();
            MockReadMapper = fixture.Freeze<Mock<IViewModelMapper<User, UserReadViewModel>>>();
            MockCreateMapper = fixture.Freeze<Mock<IViewModelMapper<UserCreateViewModel, User>>>();
            MockUpdateMapper = fixture.Freeze<Mock<IViewModelMapperUpdater<UserUpdateViewModel, User>>>();
            MockReadEnumerableMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>>>();

            MockController = new UserController(
                MockService.Object,
                MockReadMapper.Object,
                MockCreateMapper.Object,
                MockUpdateMapper.Object,
                MockReadEnumerableMapper.Object);
        }

        public UserController MockController { get; }
        public Mock<IUserService> MockService { get; }
        public Mock<IViewModelMapper<User, UserReadViewModel>> MockReadMapper { get; }
        public Mock<IViewModelMapper<UserCreateViewModel, User>> MockCreateMapper { get; }
        public Mock<IViewModelMapperUpdater<UserUpdateViewModel, User>> MockUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>> MockReadEnumerableMapper { get; }
    }
}
