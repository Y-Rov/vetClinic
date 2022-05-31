using Core.Entities;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UserCreateMapper : IViewModelMapper<UserCreateViewModel, User>
    {
        public User Map(UserCreateViewModel source)
        {
            return new User()
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                UserName = source.Email,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate
            };
        }
    }
}