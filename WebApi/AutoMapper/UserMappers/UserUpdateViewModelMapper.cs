using Core.Entities;
using Core.ViewModels.User;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.UserMappers
{
    public class UpdateViewModelToUserMapper : IViewModelMapperUpdater<UserUpdateViewModel, User>
    {
        public User Map(UserUpdateViewModel source)
        {
            return new User()
            {
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                BirthDate = source.BirthDate
            };
        }

        public void Map(UserUpdateViewModel source, User dest)
        {
            dest.FirstName = source.FirstName;
            dest.LastName = source.LastName;
            dest.UserName = source.Email;
            dest.Email = source.Email;
            dest.PhoneNumber = source.PhoneNumber;
            dest.BirthDate = source.BirthDate;
        }
    }
}
