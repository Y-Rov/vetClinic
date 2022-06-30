namespace Core.ViewModels.SalaryViewModel
{
    public class FinancialStatementForMonthViewModel
    {
        public DateViewModel Period = new DateViewModel();
        public IEnumerable<ExpencesViewModel> expences = new List<ExpencesViewModel>();
        public IEnumerable<IncomeViewModel> incomes = new List<IncomeViewModel>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }

}
