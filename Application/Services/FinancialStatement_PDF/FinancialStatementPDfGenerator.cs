using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Models.Finance;
using Core.Paginator.Parameters;

namespace Application.Services.FinancialStatement_PDF
{
    public class FinancialStatementPDfGenerator: IGenerateFullPDF<FinancialStatementParameters>
    {
        readonly IFinancialService _financialService;
        readonly ICreateTableForPDF<FinancialStatement> _createTable;
        readonly IPDfGenerator _pDfGenerator;

        public FinancialStatementPDfGenerator(
            IFinancialService financialService,
            ICreateTableForPDF<FinancialStatement> createTable, 
            IPDfGenerator pDfGenerator)
        {
            _financialService = financialService;
            _createTable = createTable;
            _pDfGenerator = pDfGenerator;
        }
        
        public async Task<PdfFileModel> GeneratePDF(FinancialStatementParameters parameters)
        {
            var financialStatementList = await _financialService.GetFinancialStatement(parameters);
            var financialStatementTable = _createTable.CreateTable(financialStatementList);
            var pdfFileParams =  _pDfGenerator.CreatePDF(financialStatementTable);

            return pdfFileParams;
        }
    }
}
