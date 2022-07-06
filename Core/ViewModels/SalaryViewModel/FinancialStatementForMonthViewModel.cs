namespace Core.ViewModels.SalaryViewModel
{
    public class FinancialStatementForMonthViewModel
    {
        //public int Id { get; set; }
        public string? Month { get; set; }
        public IEnumerable<ExpencesViewModel> ExpencesList { get; set; } = new List<ExpencesViewModel>();
        public IEnumerable<IncomeViewModel> IncomesList { get; set; } = new List<IncomeViewModel>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }

}
