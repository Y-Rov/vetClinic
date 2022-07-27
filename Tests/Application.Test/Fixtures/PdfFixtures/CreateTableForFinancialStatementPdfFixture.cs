using Application.Services.GeneratePDF.FinancialStatement_PDF;
using Core.Interfaces.Services.PDF_Service;
using Core.Models.Finance;
using Core.Paginator;

namespace Application.Test.Fixtures.PdfFixtures
{
    public class CreateTableForFinancialStatementPdfFixture
    {
        public CreateTableForFinancialStatementPdfFixture()
        {
            MockCreateTableService = new CreateTableForFinancialStatementPdf();

            FinancialStatementList = GenerateFinancialStatementList();
            FinancialStatementEmptyList = GetFinancialStatementEmptyList();
        }

        public ICreateTableForPdf<FinancialStatement> MockCreateTableService { get; }
        
        public PagedList<FinancialStatement> FinancialStatementList { get; }
        public PagedList<FinancialStatement> FinancialStatementEmptyList { get; }

        private PagedList<FinancialStatement> GenerateFinancialStatementList()
        {
            var financialStatementList = new List<FinancialStatement>
            {
                new FinancialStatement
                {
                    Month = "May,22",
                    TotalIncomes = 10,
                    TotalExpences = 20,
                    IncomesList = new List<Income>()
                    {
                        new Income
                        {
                            AppointmenId =1,
                            Cost = 5
                        }
                    },
                    ExpencesList = new List<Expences>()
                    {
                        new Expences
                        {
                            EmployeeName = "Doctor Smith",
                            SalaryValue = 10,
                            Premium = 1
                        }
                    }
                }
            };
            return new PagedList<FinancialStatement>(financialStatementList,1,1,1);
        }
        private PagedList<FinancialStatement> GetFinancialStatementEmptyList()
        {
            return new PagedList<FinancialStatement>(new List<FinancialStatement>(), 0, 1, 1);
        }

    }
}
