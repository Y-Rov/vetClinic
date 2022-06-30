namespace Core.ViewModels.SalaryViewModel
{
    public class FinancialStatementViewModel
    {
        public DateViewModel Period = new DateViewModel();
        public IEnumerable<FinancialStatementForMonthViewModel> StatementsForEachMonth = new List<FinancialStatementForMonthViewModel>();
    }
}
