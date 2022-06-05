using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SpecializationMappers
{
    public class SpecializationViewModelMapper : IViewModelMapper<Specialization, SpecializationViewModel>
    {
        public SpecializationViewModel Map(Specialization source)
        {
            return new SpecializationViewModel()
            {
                Id = source.Id,
                Name = source.Name
            };
        }
        //public IEnumerable<SpecializationReadViewModel> Map(IEnumerable<Specialization> source)
        //{
        //    List<SpecializationReadViewModel> specializations = new();
        //    foreach (var spec in source)
        //        specializations.Add(Map(spec));
        //    return specializations;
        //}
    }
}
