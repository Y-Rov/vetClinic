using Core.Interfaces.Services.PDF_Service;
using Core.Models.Finance;
using Core.Paginator;
using System.Data;

namespace Application.Services.FinancialStatement_PDF
{
    public class CreateTableForFinancialStatementPDF : ICreateTableForPDF<FinancialStatement>
    {
        public DataTable CreateTable(PagedList<FinancialStatement> listOfFinancialStatement)
        {
            var table = new DataTable();
            return table;
        }
    }
}
