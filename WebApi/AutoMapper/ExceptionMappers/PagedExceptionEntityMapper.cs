using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.ExceptionViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ExceptionMappers
{
    public class PagedExceptionEntityMapper : IViewModelMapper<PagedList<ExceptionEntity>, PagedReadViewModel<ExceptionEntityReadViewModel>>
    {
        private readonly IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>> _exceptionModel;
        public PagedExceptionEntityMapper(IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>> exceptionModel)
        {
            _exceptionModel = exceptionModel;
        }

        public PagedReadViewModel<ExceptionEntityReadViewModel> Map(PagedList<ExceptionEntity> source)
        {
            return new PagedReadViewModel<ExceptionEntityReadViewModel>()
            {
                CurrentPage = source.CurrentPage,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                HasPrevious = source.HasPrevious,
                HasNext = source.HasNext,
                TotalPages = source.TotalPages,
                ExceptionList = _exceptionModel.Map(source.AsEnumerable())
            };
        }
       
    }


}
