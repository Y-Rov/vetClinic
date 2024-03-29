﻿using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;

namespace Application.Test.Fixtures
{
    public class UserServiceFixture
    {
        public UserServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockUserService = fixture.Freeze<UserService>();
            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
            MockUserProfilePictureService = fixture.Freeze<Mock<IUserProfilePictureService>>();

            MockUserService = new UserService(
                MockUserRepository.Object,
                MockLoggerManager.Object,
                MockUserProfilePictureService.Object);

            Id = 1;
            Role = "Client";
            Password = "test_pass";
            SpecializationName = "test_spec";
            User = GetUser();
            UserParameters = GetUserParameters();
            Specialization = GetSpecialization();
            UserSpecializations = GetUserSpecializations();
            Users = GetUsers();
            PagedUsers = GetPagedUsers();
        }

        public UserService MockUserService { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public Mock<IUserProfilePictureService> MockUserProfilePictureService { get; set; }

        public int Id { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string SpecializationName { get; set; }
        public UserParameters UserParameters { get; set; }
        public Specialization Specialization { get; set; }
        public IEnumerable<UserSpecialization> UserSpecializations { get; set; }
        public User User { get; set; }
        public List<User> Users { get; set; }
        public PagedList<User> PagedUsers { get; set; }

        private User GetUser()
        {
            return new User()
            {
                Id = Id,
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

        private Specialization GetSpecialization()
        {
            return new Specialization()
            {
                Name = SpecializationName
            };
        }

        private List<UserSpecialization> GetUserSpecializations()
        {
            return new List<UserSpecialization>()
            {
                new UserSpecialization()
                {
                    Specialization = GetSpecialization()
                }
            };
        }

        private PagedList<User> GetPagedUsers()
        {
            return new PagedList<User>(GetUsers(), 5, 1, 5);
        }
    }
}
