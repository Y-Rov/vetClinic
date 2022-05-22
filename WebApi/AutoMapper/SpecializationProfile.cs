using AutoMapper;
using Core.DTO;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
