using Application.Test.Fixtures.PdfFixtures;
using Core.Entities;
using Core.Paginator;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.PdfTests
{
    public class CreateTableForAnimalMedCardPDFTests : IClassFixture<CreateTableForAnimalMedCardPdfFixture>
    {
        public CreateTableForAnimalMedCardPDFTests(CreateTableForAnimalMedCardPdfFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly CreateTableForAnimalMedCardPdfFixture _fixture;

        [Fact]
        public void GetTable_ShouldReturnFilledTable()
        {
            //Act
            var actualResult = _fixture.MockAnimalMedCardTableCreater.CreateTable(_fixture.ListOfAppointments);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_fixture.ExpectedTable.Rows.Count, actualResult.Rows.Count);
            Assert.Equal(_fixture.ExpectedTable.Columns.Count, actualResult.Columns.Count);
        }

        [Fact]
        public void GetTable_ShouldReturnEmptyTable()
        {
            //Act
            var actualResult = _fixture.MockAnimalMedCardTableCreater.CreateTable(_fixture.EmpyListOfAppointments);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_fixture.ExpectedEmptyTable.Rows.Count, actualResult.Rows.Count);
            Assert.Equal(_fixture.ExpectedEmptyTable.Columns.Count, actualResult.Columns.Count);
        }
    }
}
