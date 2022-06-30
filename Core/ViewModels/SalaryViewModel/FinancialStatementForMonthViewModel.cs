using Core.Entities;
using Core.Models;
using Core.ViewModels.ProcedureViewModels;

namespace Core.ViewModels.SalaryViewModel
{
    public class DateViewModel
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
    public class ExpencesViewModel
    {
        public string EmployeeName { get; set; }
        public decimal SalaryValue { get; set; }
        public decimal Premium { get; set; }
    }

    public class IncomeViewModel
    {
        public int AppointmenId { get; set; }
        public IEnumerable<ProcedureReadViewModel> ListOfProcedures = new List<ProcedureReadViewModel>();
        public decimal Cost { get; set; }
    }
    public class FinancialStatementForMonthViewModel
    {
        public DateViewModel Period = new DateViewModel();
        public IEnumerable<ExpencesViewModel> expences = new List<ExpencesViewModel>();
        public IEnumerable<IncomeViewModel> incomes = new List<IncomeViewModel>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }
    public class FinancialStatementViewModel
    {
        public DateViewModel Period = new DateViewModel();
        public IEnumerable<FinancialStatementForMonthViewModel> StatementsForEachMonth = new List<FinancialStatementForMonthViewModel>();
    }
}
