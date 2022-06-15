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
            string? address = null;

            //if (source.Address is not null)
            //{
            //    address = string.Join(", ", source.Address.GetType()
            //        .GetProperties().Select(p => p.Name));
            //}

            if (source.Address is not null)
            {
                // ZipCode and AppartmentNumber are not required, so check for trailing ', '
                // and think how to check if they are nto empty
                address = $"{source.Address.City}, {source.Address.Street}, {source.Address.House}, " +
                    $"{source.Address.ApartmentNumber}, {source.Address.ZipCode}";
            }

            return new UserReadViewModel()
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate,
                Role = role,
                Salary = source.Salary?.Value,
                Address = address,
                Portfolio = source.Portfolio?.Description
            };
        }
    }
}