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
            string? address = GetAddress(source);
            var specializations = source.UserSpecializations.Select(s => s.Specialization?.Name!);

            return new UserReadViewModel()
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate,
                Role = role,
                Address = address,
                Portfolio = source.Portfolio?.Description,
                Specializations = specializations
            };
        }

        private string? GetAddress(User user)
        {
            string? address = null;

            if (user.Address is not null)
            {
                address = $"{user.Address.City}, {user.Address.Street}, {user.Address.House}, " +
                    $"{user.Address.ApartmentNumber}, {user.Address.ZipCode}";
                address = address.Trim(new char[] { ',', ' ' });
            }

            return address;
        }
    }
}