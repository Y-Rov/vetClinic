using Application.Test.Fixtures.PdfFixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.PdfTests
{
    public class CreateTableForFinancialStatementPdfTest : IClassFixture<CreateTableForFinancialStatementPdfFixture>
    {
        public CreateTableForFinancialStatementPdfTest(CreateTableForFinancialStatementPdfFixture fixture)
        {
            _fixture = fixture;
        }
        readonly CreateTableForFinancialStatementPdfFixture _fixture;

        [Fact]
        public void GetTable_whenFinancialStatementListIsNotEmpty_ReturnFilledTable()
        {
            //Act

            var result = _fixture.MockCreateTableService.CreateTable(_fixture.FinancialStatementList);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(2,result.Rows.Count);
        }

        [Fact]
        public void GetTable_whenFinancialStatementListIsEmpty_ReturnEmptyTable()
        {
            //Act

            var result = _fixture.MockCreateTableService.CreateTable(_fixture.FinancialStatementEmptyList);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(3, result.Columns.Count);
            Assert.Equal(0, result.Rows.Count);
        }
    }
}
