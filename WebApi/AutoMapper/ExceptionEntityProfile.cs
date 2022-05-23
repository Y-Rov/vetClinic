using AutoMapper;
using Core.ViewModel;
using Core.Entities;

namespace WebApi.AutoMapper
{
    public class ExceptionEntityProfile : Profile
    {
        public ExceptionEntityProfile()
        {
            CreateMap<ExceptionEntity, ExceptionEntityReadViewModel>();
        }
    }
}
