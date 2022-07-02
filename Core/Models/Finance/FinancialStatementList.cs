namespace Core.Models.Finance
{
    public class FinancialStatementList
    {
        public Date Period { get; set; } = new Date();
        public IList<FinancialStatement> StatementsForEachMonth { get; set; } = new List<FinancialStatement>();
    }
}

