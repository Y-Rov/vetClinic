using Core.Models;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.ExceptionViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ExceptionMappers
{
    public class PagedExceptionStatsMapper : IViewModelMapper<PagedList<ExceptionStats>, PagedReadViewModel<ExceptionStatsReadViewModel>>
    {
        private readonly IEnumerableViewModelMapper<IEnumerable<ExceptionStats>, IEnumerable<ExceptionStatsReadViewModel>> _exceptionModel;
        public PagedExceptionStatsMapper(IEnumerableViewModelMapper<IEnumerable<ExceptionStats>, IEnumerable<ExceptionStatsReadViewModel>> exceptionModel)
        {
            _exceptionModel = exceptionModel;
        }
        public PagedReadViewModel<ExceptionStatsReadViewModel> Map(PagedList<ExceptionStats> source)
        {
            return new PagedReadViewModel<ExceptionStatsReadViewModel>()
            {
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                TotalPages = source.TotalPages,
                Entities = _exceptionModel.Map(source.AsEnumerable())
            };
        }
    }
}
