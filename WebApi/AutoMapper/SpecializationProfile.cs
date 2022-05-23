using AutoMapper;
using Core.DTO;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class SpecializationProfile : Profile
    {
        public SpecializationProfile()
        {
            CreateMap<Specialization, SpecializationDTO>()
                .ForMember(specialization => specialization.Id, input =>
                    input.MapFrom(specDTO => specDTO.Id))
                .ReverseMap();
        }
    }
}
