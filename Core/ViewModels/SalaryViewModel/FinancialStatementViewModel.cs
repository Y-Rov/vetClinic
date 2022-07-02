namespace Core.ViewModels.SalaryViewModel
{
    public class FinancialStatementViewModel
    {
        public DateViewModel Period { get; set; }
        public IEnumerable<FinancialStatementForMonthViewModel> StatementsForEachMonth { get; set; } = new List<FinancialStatementForMonthViewModel>();
    }
}
