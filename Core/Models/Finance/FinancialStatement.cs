namespace Core.Models.Finance
{
    public class FinancialStatement
    {
        //public Date Period { get; set; } = new Date();
        public string Month { get; set; }
        public IEnumerable<Expences> expences { get; set; } = new List<Expences>();
        public IEnumerable<Income> incomes { get; set; } = new List<Income>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }
}
