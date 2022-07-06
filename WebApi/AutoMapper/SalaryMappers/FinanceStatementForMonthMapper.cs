using Core.Models.Finance;
using Core.ViewModels.SalaryViewModel;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.SalaryMappers
{
    public class FinancialStatementForMonthMapper : IEnumerableViewModelMapper<IEnumerable<FinancialStatement>,
        IEnumerable<FinancialStatementForMonthViewModel>>
    {

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
                Month = source.Month,
                IncomesList = source.IncomesList,
                ExpencesList = source.ExpencesList
            };

            return financialStatementForMonthViewModel;
        }
    }
}
