using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class SalaryPagedMapper: IViewModelMapper<PagedList<Salary>, PagedReadViewModel<SalaryViewModel>>
    {
        readonly IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>>
            _listMapper;

        public SalaryPagedMapper(IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>> listMapper)
        {
            _listMapper = listMapper;
        }

        public PagedReadViewModel<SalaryViewModel> Map(PagedList<Salary> source)
        {
            return new PagedReadViewModel<SalaryViewModel>
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
