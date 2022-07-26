using Application.Test.Fixtures.PdfFixtures;
using Core.Models.Finance;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;
using System.Data;

namespace Application.Test.PdfTests
{
    public class FinancialStatementPdfGeneratorTest:IClassFixture<FinancialStatementPdfGeneratorFixture>
    {
        readonly FinancialStatementPdfGeneratorFixture _fixture;

        public FinancialStatementPdfGeneratorTest(FinancialStatementPdfGeneratorFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetNormalFile_ShouldReturnNormalPdf()
        {
            //Arrange
            _fixture.MockFinancialService
                .Setup(ser => ser.GetFinancialStatement(It.IsAny<FinancialStatementParameters>()))
                .ReturnsAsync(_fixture.FinancialStatementList);

            _fixture.MockFinancialStatementCreateTable
                .Setup(ser => ser.CreateTable(It.IsAny<PagedList<FinancialStatement>>()))
                .Returns(_fixture.TableWithFinancialStatement);

            _fixture.MockPdfGenerator
                .Setup(ser => ser.CreatePdf(It.IsAny<DataTable>()))
                .Returns(_fixture.PdfFile);

            //Act
            var actualResult = await _fixture.MockGenerateFinancialStatementPdfService.GeneratePdf(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_fixture.PdfFile.FileStream.Length, actualResult.FileStream.Length);
        }

        [Fact]
        public async Task GetEmptyFile_ShouldReturnEmptyPdf()
        {
            //Arrange
            _fixture.MockFinancialService
                .Setup(ser => ser.GetFinancialStatement(It.IsAny<FinancialStatementParameters>()))
                .ReturnsAsync(_fixture.FinancialStatementEmptyList);

            _fixture.MockFinancialStatementCreateTable
                .Setup(ser => ser.CreateTable(It.IsAny<PagedList<FinancialStatement>>()))
                .Returns(_fixture.EmptyTable);

            _fixture.MockPdfGenerator
                .Setup(ser => ser.CreatePdf(It.IsAny<DataTable>()))
                .Returns(_fixture.PdfFile);

            //Act
            var actualResult = await _fixture.MockGenerateFinancialStatementPdfService.GeneratePdf(_fixture.FinancialStatementParameters);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_fixture.PdfFile.FileStream.Length, actualResult.FileStream.Length);
        }
    }
}
