using Application.Services.GeneratePDF.FinancialStatement_PDF;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Models.Finance;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;
using System.Data;

namespace Application.Test.Fixtures.PdfFixtures
{
    public class FinancialStatementPdfGeneratorFixture
    {
        public FinancialStatementPdfGeneratorFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockFinancialService = fixture.Freeze<Mock<IFinancialService>>();
            MockFinancialStatementCreateTable = fixture.Freeze<Mock<ICreateTableForPdf<FinancialStatement>>>();
            MockPdfGenerator = fixture.Freeze<Mock<IPdfGenerator>>();

            MockGenerateFinancialStatementPdfService = new FinancialStatementPDfGenerator(
                MockFinancialService.Object,
                MockFinancialStatementCreateTable.Object,
                MockPdfGenerator.Object);

            FinancialStatementList = GenerateFinancialStatementList();
            FinancialStatementEmptyList = GetFinancialStatementEmptyList();
            FinancialStatementParameters = GenerateFinancialStatementParameters();
            TableWithFinancialStatement = GenerateTableWithFinancialStatement();
            EmptyTable = GenerateEmptyTable();
            PdfFile = GeneratePdfFileModel();
        }

        public FinancialStatementPDfGenerator MockGenerateFinancialStatementPdfService { get; }
        public Mock<IFinancialService> MockFinancialService { get; }
        public Mock<ICreateTableForPdf<FinancialStatement>> MockFinancialStatementCreateTable { get; }
        public Mock<IPdfGenerator> MockPdfGenerator { get; }

        public PagedList<FinancialStatement> FinancialStatementList { get; }
        public PagedList<FinancialStatement> FinancialStatementEmptyList { get; }
        public FinancialStatementParameters FinancialStatementParameters {get;}
        public DataTable TableWithFinancialStatement { get;}
        public DataTable EmptyTable { get; }
        public PdfFileModel PdfFile { get; }

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
            return new PagedList<FinancialStatement>(financialStatementList, 1, 1, 1);
        }
        private PagedList<FinancialStatement> GetFinancialStatementEmptyList()
        {
            return new PagedList<FinancialStatement>(new List<FinancialStatement>(), 0, 1, 1);
        }
        private FinancialStatementParameters GenerateFinancialStatementParameters()
        {
            var date = new DatePeriod()
            {
                StartDate = new DateTime(2022, 5, 1),
                EndDate = new DateTime(2022, 6, 1)
            };
            var res = new FinancialStatementParameters()
            {
                PageNumber = 1,
                PageSize = 5,
                Date = date
            };
            return res;
        }
        private DataTable GenerateEmptyTable()
        {
            var table = new DataTable();
            return table;
        }
        private DataTable GenerateTableWithFinancialStatement()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Month");
            table.Rows.Add("May, 2022");

            return table;
        }
        private PdfFileModel GeneratePdfFileModel()
        {
            MemoryStream stream = new MemoryStream();
            stream.Position = 0;
            var pdfFileParam = new PdfFileModel
            {
                ContentType = "application/pdf",
                DefaultFileName = "pdf.pdf",
                FileStream = stream
            };

            return pdfFileParam;
        }
    }
}
