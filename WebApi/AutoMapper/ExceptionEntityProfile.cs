using AutoMapper;
using Core.DTO;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class ExceptionEntityProfile : Profile
    {
        public ExceptionEntityProfile()
        {
            CreateMap<ExceptionEntity, ExceptionEntityReadDto>();
        }
    }
}
