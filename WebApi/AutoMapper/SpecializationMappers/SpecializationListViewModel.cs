using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SpecializationMappers
{
    public class SpecializationListViewModel : IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>>
    {
        IViewModelMapper<Specialization, SpecializationViewModel> _entityMapper;

        public SpecializationListViewModel(IViewModelMapper<Specialization, SpecializationViewModel> entityMapper)
        {
            _entityMapper = entityMapper;
        }

        public IEnumerable<SpecializationViewModel> Map(IEnumerable<Specialization> source)
        {
            foreach(var specialization in source)
                yield return _entityMapper.Map(specialization);
        }
    }
}
