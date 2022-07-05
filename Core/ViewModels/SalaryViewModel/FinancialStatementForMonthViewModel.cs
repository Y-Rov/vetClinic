namespace Core.ViewModels.SalaryViewModel
{
    public class FinancialStatementForMonthViewModel
    {
        public int id { get; set; }
        public string? Month { get; set; }
        public IEnumerable<ExpencesViewModel> expences { get; set; } = new List<ExpencesViewModel>();
        public IEnumerable<IncomeViewModel> incomes { get; set; } = new List<IncomeViewModel>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }

}
