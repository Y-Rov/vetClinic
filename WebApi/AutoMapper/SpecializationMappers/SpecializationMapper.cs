using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SpecializationMappers
{
    public class SpecializationMapper : IViewModelMapper<SpecializationViewModel, Specialization>
    {
        public Specialization Map(SpecializationViewModel source)
        {
            return new Specialization()
            {
                Name = source.Name
            };
        }
    }
}
