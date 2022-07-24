using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.FeedbackViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.FeedbackMappers
{
    public class FeedbackPagedMapper : IViewModelMapper<PagedList<Feedback>, PagedReadViewModel<FeedbackReadViewModel>>
    {
        private readonly IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> _listMapper;

        public FeedbackPagedMapper(IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> listMapper)
        {
            _listMapper = listMapper;
        }

        public PagedReadViewModel<FeedbackReadViewModel> Map(PagedList<Feedback> source)
        {
            return new PagedReadViewModel<FeedbackReadViewModel>
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
