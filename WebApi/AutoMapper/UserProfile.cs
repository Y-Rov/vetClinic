using AutoMapper;
using Core.ViewModel;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>()
                .AfterMap((dto, u) => u.UserName = u.Email);
            CreateMap<UserUpdateDto, User>()
                .AfterMap((dto, u) => u.UserName = u.Email);
        }
    }
}