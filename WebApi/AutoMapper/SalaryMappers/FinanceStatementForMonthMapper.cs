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
                TotalExpences = Math.Round(source.TotalExpences,2),
                TotalIncomes = Math.Round(source.TotalIncomes,2),
                Month = source.Month,
                IncomesList = source.IncomesList,
                ExpencesList = source.ExpencesList
            };
            foreach(var expence in financialStatementForMonthViewModel.ExpencesList)
            {
                expence.SalaryValue = Math.Round(expence.SalaryValue,2);
                expence.Premium = Math.Round(expence.Premium,2);
            }

            return financialStatementForMonthViewModel;
        }
    }
}
