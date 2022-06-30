namespace Core.Models.Finance
{
    public class FinancialStatement
    {
        public Date Period = new Date();
        public IEnumerable<Expences> expences = new List<Expences>();
        public IEnumerable<Income> incomes = new List<Income>();
        public decimal TotalExpences { get; set; }
        public decimal TotalIncomes { get; set; }
    }
}
