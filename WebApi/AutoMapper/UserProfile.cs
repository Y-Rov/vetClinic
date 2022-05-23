using AutoMapper;
using Core.ViewModel;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadViewModel>();
            CreateMap<UserCreateViewModel, User>()
                .AfterMap((dto, u) => u.UserName = u.Email);
            CreateMap<UserUpdateViewModel, User>()
                .AfterMap((dto, u) => u.UserName = u.Email);
        }
    }
}