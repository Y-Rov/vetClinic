using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Models.Finance;
using Core.Paginator.Parameters;

namespace Application.Services.GeneratePDF.FinancialStatement_PDF
{
    public class FinancialStatementPDfGenerator: IGenerateFullPdf<FinancialStatementParameters>
    {
        readonly IFinancialService _financialService;
        readonly ICreateTableForPdf<FinancialStatement> _createTable;
        readonly IPdfGenerator _pDfGenerator;

        public FinancialStatementPDfGenerator(
            IFinancialService financialService,
            ICreateTableForPdf<FinancialStatement> createTable, 
            IPdfGenerator pDfGenerator)
        {
            _financialService = financialService;
            _createTable = createTable;
            _pDfGenerator = pDfGenerator;
        }
        
        public async Task<PdfFileModel> GeneratePdf(FinancialStatementParameters parameters)
        {
            var financialStatementList = await _financialService.GetFinancialStatement(parameters);
            var financialStatementTable = _createTable.CreateTable(financialStatementList);
            var pdfFileParams =  _pDfGenerator.CreatePdf(financialStatementTable);

            return pdfFileParams;
        }
    }
}
