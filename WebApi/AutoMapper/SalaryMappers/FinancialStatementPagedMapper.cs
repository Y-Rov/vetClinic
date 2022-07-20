using Core.Models.Finance;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class FinancialStatementPagedMapper : IViewModelMapper<PagedList<FinancialStatement>, PagedReadViewModel<FinancialStatementForMonthViewModel>>
    {
        readonly IViewModelMapper<IEnumerable<FinancialStatement>, IEnumerable<FinancialStatementForMonthViewModel>>
            _listMapper;

        public FinancialStatementPagedMapper(IViewModelMapper<IEnumerable<FinancialStatement>, IEnumerable<FinancialStatementForMonthViewModel>> listMapper)
        {
            _listMapper = listMapper;
        }

        public PagedReadViewModel<FinancialStatementForMonthViewModel> Map(PagedList<FinancialStatement> source)
        {
            return new PagedReadViewModel<FinancialStatementForMonthViewModel>
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
