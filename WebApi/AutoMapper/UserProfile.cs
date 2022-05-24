using AutoMapper;
using Core.Entities;
using Core.ViewModels.User;

namespace WebApi.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadViewModel>();
            CreateMap<UserCreateViewModel, User>()
                .AfterMap((vm, u) => u.UserName = u.Email);
            CreateMap<UserUpdateViewModel, User>()
                .AfterMap((vm, u) => u.UserName = u.Email);
        }
    }
}