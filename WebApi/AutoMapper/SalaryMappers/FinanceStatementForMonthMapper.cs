using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class FinancialStatementForMonthMapper : IViewModelMapper<IEnumerable<FinancialStatement>,
        IEnumerable<FinancialStatementForMonthViewModel>>
    {
        private readonly IViewModelMapper<Date, DateViewModel> _dateMapper;
        private readonly IViewModelMapper<IEnumerable<Income>, IEnumerable<IncomeViewModel>> _incomeMapper;
        private readonly IViewModelMapper<IEnumerable<Expences>, IEnumerable<ExpencesViewModel>> _expencesMapper;


        public FinancialStatementForMonthMapper(
            IViewModelMapper<IEnumerable<Income>, IEnumerable<IncomeViewModel>> incomeMapper,
            IViewModelMapper<IEnumerable<Expences>, IEnumerable<ExpencesViewModel>> expencesMapper,
            IViewModelMapper<Date, DateViewModel> dateMapper)
        {
            _incomeMapper = incomeMapper;
            _expencesMapper = expencesMapper;
            _dateMapper = dateMapper;
        }

        public IEnumerable<FinancialStatementForMonthViewModel> Map(IEnumerable<FinancialStatement> source)
        {
            var listViewModel = source.Select(GetFinStatViewModel).ToList();
            return listViewModel;
        }

        public FinancialStatementForMonthViewModel GetFinStatViewModel(FinancialStatement source)
        {
            var financialStatementForMonthViewModel = new FinancialStatementForMonthViewModel()
            {
                TotalExpences = source.TotalExpences,
                TotalIncomes = source.TotalIncomes
            };
            financialStatementForMonthViewModel.Period = _dateMapper.Map(source.Period);
            financialStatementForMonthViewModel.incomes = _incomeMapper.Map(source.incomes);
            financialStatementForMonthViewModel.expences = _expencesMapper.Map(source.expences);
            return financialStatementForMonthViewModel;
        }
    }
}
