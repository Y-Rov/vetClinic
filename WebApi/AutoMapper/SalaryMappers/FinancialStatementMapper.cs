using Core.Entities;
using Core.Models.Finance;
using Core.ViewModels;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class FinancialStatementMapper: IViewModelMapper<FinancialStatementList, FinancialStatementViewModel>
    {
        readonly IViewModelMapper<IEnumerable<FinancialStatement>, IEnumerable<FinancialStatementForMonthViewModel>> _finStatMapper;
        readonly IViewModelMapper<Date, DateViewModel> _dateMapper;

        public FinancialStatementMapper(
            IViewModelMapper<IEnumerable<FinancialStatement>, IEnumerable<FinancialStatementForMonthViewModel>> finStatMapper,
            IViewModelMapper<Date,DateViewModel> dateMapper)
        {
            _finStatMapper = finStatMapper;
            _dateMapper = dateMapper;
        }

        public FinancialStatementViewModel Map (FinancialStatementList source)
        {
            var viewModel = new FinancialStatementViewModel();
            viewModel.Period = _dateMapper.Map(source.Period);
            viewModel.StatementsForEachMonth = _finStatMapper.Map(source.StatementsForEachMonth);
            return viewModel;
        }
    }
}
