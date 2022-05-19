using AutoMapper;
using Core.DTO;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap(); //reverse so the both direction
        }
    }
}
