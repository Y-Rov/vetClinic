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

            MockUserService = fixture.Freeze<Mock<IUserService>>();
            MockReadMapper = fixture.Freeze<Mock<IViewModelMapper<User, UserReadViewModel>>>();
            MockCreateMapper = fixture.Freeze<Mock<IViewModelMapper<UserCreateViewModel, User>>>();
            MockUpdateMapper = fixture.Freeze<Mock<IViewModelMapperUpdater<UserUpdateViewModel, User>>>();
            MockReadEnumerableMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>>>();

            MockUserController = new UserController(
                MockUserService.Object,
                MockReadMapper.Object,
                MockCreateMapper.Object,
                MockUpdateMapper.Object,
                MockReadEnumerableMapper.Object);
            
            Id = 1;
            User = new() { Id = Id, FirstName = "Ren", LastName = "Amamiya" };
            ReadViewModel = new() { Id = Id, FirstName = "Ren", LastName = "Amamiya" };
            CreateViewModel = new() { FirstName = "Ren", LastName = "Amamiya", Password = "test_pass" };
            UpdateViewModel = new() { FirstName = "Ren", LastName = "Amamiya" };
            Users = new List<User>() { User };
            ReadViewModels = new List<UserReadViewModel>() { ReadViewModel };
        }

        public UserController MockUserController { get; }
        public Mock<IUserService> MockUserService { get; }
        public Mock<IViewModelMapper<User, UserReadViewModel>> MockReadMapper { get; }
        public Mock<IViewModelMapper<UserCreateViewModel, User>> MockCreateMapper { get; }
        public Mock<IViewModelMapperUpdater<UserUpdateViewModel, User>> MockUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>> MockReadEnumerableMapper { get; }

        public int Id { get; set; }
        public User User { get; set; }
        public UserReadViewModel ReadViewModel { get; set; }
        public UserCreateViewModel CreateViewModel { get; set; }
        public UserUpdateViewModel UpdateViewModel { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<UserReadViewModel> ReadViewModels { get; set; }
    }
}
