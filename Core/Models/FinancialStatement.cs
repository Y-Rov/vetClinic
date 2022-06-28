using Core.Entities;

namespace Core.Models
{
    public class Date
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }

    public class Expences
    {
        public string EmployeeName { get; set; }
        public decimal SalaryValue { get; set; }
        public decimal Premium { get; set; }
    }

    public class Income
    {
        public int AppointmenId { get; set; }
        public IEnumerable<Procedure> ListOfProcedures = new List<Procedure>();
        public decimal Cost { get; set; }
    }
    public class FinancialStatement
    {
        public Date Period = new Date();
        public IEnumerable<Expences> expences = new List<Expences>();
        public IEnumerable<Income> incomes = new List<Income>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }
}

