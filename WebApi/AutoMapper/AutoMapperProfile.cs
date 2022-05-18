using AutoMapper;
using Core.DTO;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap(); //reverse so the both direction
        }
    }
}
