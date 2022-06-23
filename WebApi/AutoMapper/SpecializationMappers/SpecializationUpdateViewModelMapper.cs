using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SpecializationMappers
{
    public class SpecializationUpdateViewModelMapper : IViewModelMapper<SpecializationUpdateViewModel, Specialization>
    {
        public Specialization Map(SpecializationUpdateViewModel source)
        {
            return new Specialization
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}
