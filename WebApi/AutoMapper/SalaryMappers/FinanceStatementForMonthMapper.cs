using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class FinancialStatementForMonthMapper : IEnumerableViewModelMapper<IEnumerable<FinancialStatement>,
        IEnumerable<FinancialStatementForMonthViewModel>>
    {
        private readonly IViewModelMapper<IEnumerable<Income>, IEnumerable<IncomeViewModel>> _incomeMapper;
        private readonly IViewModelMapper<IEnumerable<Expences>, IEnumerable<ExpencesViewModel>> _expencesMapper;


        public FinancialStatementForMonthMapper(
            IViewModelMapper<IEnumerable<Income>, IEnumerable<IncomeViewModel>> incomeMapper,
            IViewModelMapper<IEnumerable<Expences>, IEnumerable<ExpencesViewModel>> expencesMapper)
        {
            _incomeMapper = incomeMapper;
            _expencesMapper = expencesMapper;
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
                TotalIncomes = source.TotalIncomes,
                Month = source.Month
            };
            financialStatementForMonthViewModel.IncomesList = _incomeMapper.Map(source.IncomesList);
            financialStatementForMonthViewModel.ExpencesList = _expencesMapper.Map(source.ExpencesList);
            return financialStatementForMonthViewModel;
        }
    }
}
