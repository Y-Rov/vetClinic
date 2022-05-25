using Core.Entities;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserToUserReadViewModelMapper : IViewModelMapper<User, UserReadViewModel>
    {
        private readonly UserManager<User> _userManager;

        public UserToUserReadViewModelMapper(UserManager<User> userManager)
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
                Role = role
            };
        }
    }
}