namespace Core.Models.Finance
{
    public class FinancialStatementList
    {
        public Date Period = new Date();
        public IEnumerable<FinancialStatement> StatementsForEachMonth = new List<FinancialStatement>();
    }
}

