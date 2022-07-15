using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.SpecializationViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SpecializationMappers
{
    public class SpecializationPagedMapper : IViewModelMapper<PagedList<Specialization>, PagedReadViewModel<SpecializationViewModel>>
    {

        readonly IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>> 
            _listMapper;

        public SpecializationPagedMapper(IViewModelMapper<IEnumerable<Specialization>, IEnumerable<SpecializationViewModel>> listMapper)
        {
            _listMapper = listMapper;
        }

        public PagedReadViewModel<SpecializationViewModel> Map(PagedList<Specialization> source)
        {
            return new PagedReadViewModel<SpecializationViewModel>
            {
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                TotalPages = source.TotalPages,
                Entities = _listMapper.Map(source.AsEnumerable())
            };
        }
    }
}
