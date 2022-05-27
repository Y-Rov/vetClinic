using Core.Entities;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserReadMapper : IViewModelMapper<User, UserReadViewModel>
    {
        private readonly UserManager<User> _userManager;

        public UserReadMapper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public UserReadViewModel Map(User source)
        {
            string? role = _userManager.GetRolesAsync(source).Result.SingleOrDefault();

            return new UserReadViewModel()
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate,
                Role = role
            };
        }
    }
}