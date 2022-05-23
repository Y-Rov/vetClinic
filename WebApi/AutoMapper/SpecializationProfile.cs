using AutoMapper;
using Core.ViewModel;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class SpecializationProfile : Profile
    {
        public SpecializationProfile()
        {
            CreateMap<Specialization, SpecializationViewModel>()
                .ForMember(specialization => specialization.Id, input =>
                    input.MapFrom(specDTO => specDTO.Id))
                .ReverseMap();
        }
    }
}
