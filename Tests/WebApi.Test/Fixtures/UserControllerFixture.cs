using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
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
            MockReadPagedMapper = fixture.Freeze<Mock<IViewModelMapper<PagedList<User>, PagedReadViewModel<UserReadViewModel>>>>();

            MockUserController = new UserController(
                MockUserService.Object,
                MockReadMapper.Object,
                MockCreateMapper.Object,
                MockUpdateMapper.Object,
                MockReadEnumerableMapper.Object,
                MockReadPagedMapper.Object);
            
            Id = 1;
            User = GetUser();
            ReadViewModel = GetUserReadViewModel();
            CreateViewModel = GetUserCreateViewModel();
            UpdateViewModel = GetUserUpdateViewModel();
            UserParameters = GetUserParameters();
            Users = GetUsers();
            ReadViewModels = GetReadViewModels();
            PagedList = GetPagedUsers();
            PagedReadViewModels = GetPagedReadViewModel();
        }

        public UserController MockUserController { get; }
        public Mock<IUserService> MockUserService { get; }
        public Mock<IViewModelMapper<User, UserReadViewModel>> MockReadMapper { get; }
        public Mock<IViewModelMapper<UserCreateViewModel, User>> MockCreateMapper { get; }
        public Mock<IViewModelMapperUpdater<UserUpdateViewModel, User>> MockUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<User>, IEnumerable<UserReadViewModel>>> MockReadEnumerableMapper { get; }
        public Mock<IViewModelMapper<PagedList<User>, PagedReadViewModel<UserReadViewModel>>> MockReadPagedMapper { get; }

        public int Id { get; set; }
        public User User { get; set; }
        public UserReadViewModel ReadViewModel { get; set; }
        public UserCreateViewModel CreateViewModel { get; set; }
        public UserUpdateViewModel UpdateViewModel { get; set; }
        public UserParameters UserParameters { get; set; }
        public List<User> Users { get; set; }
        public List<UserReadViewModel> ReadViewModels { get; set; }
        public PagedReadViewModel<UserReadViewModel> PagedReadViewModels { get; set; }
        public PagedList<User> PagedList { get; set; }

        private User GetUser()
        {
            return new User()
            {
                Id = Id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };
        }

        private UserReadViewModel GetUserReadViewModel()
        {
            return new UserReadViewModel()
            {
                Id = Id,
                FirstName = "Ren",
                LastName = "Amamiya"
            };
        }

        private UserCreateViewModel GetUserCreateViewModel()
        {
            return new UserCreateViewModel()
            {
                FirstName = "Ren",
                LastName = "Amamiya",
                Password = "test_pass"
            };
        }

        private UserUpdateViewModel GetUserUpdateViewModel()
        {
            return new UserUpdateViewModel()
            {
                FirstName = "Ren",
                LastName = "Amamiya"
            };
        }

        private List<User> GetUsers()
        {
            return new List<User>()
            {
                GetUser(),
                GetUser()
            };
        }

        private List<UserReadViewModel> GetReadViewModels()
        {
            return new List<UserReadViewModel>()
            {
                GetUserReadViewModel(),
                GetUserReadViewModel()
            };
        }

        private UserParameters GetUserParameters()
        {
            return new UserParameters()
            {
                FilterParam = "Ophthalmology",
                OrderByParam = "FirstName",
                PageNumber = 1,
                PageSize = 5
            };
        }

        private PagedList<User> GetPagedUsers()
        {
            return new PagedList<User>(GetUsers(), 5, 1, 5);
        }

        private PagedReadViewModel<UserReadViewModel> GetPagedReadViewModel()
        {
            return new PagedReadViewModel<UserReadViewModel>()
            {
                Entities = GetReadViewModels()
            };
        }
    }
}
