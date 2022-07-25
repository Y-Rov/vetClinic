using Application.Test.Fixtures.PdfFixtures;
using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.PdfTests
{
    public class AnimalMedCardPDFGeneratorTests : IClassFixture<AnimalMedCardPdfGeneratorFixture>
    {
        public AnimalMedCardPDFGeneratorTests(AnimalMedCardPdfGeneratorFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly AnimalMedCardPdfGeneratorFixture _fixture;

        [Fact]
        public void GetFilledTable_ShouldReturnTable()
        {
            //Arrange
            _fixture.MockAnimalCreateTable
                .Setup(ser => ser.CreateTable(It.IsAny<PagedList<Appointment>>()))
                .Returns(_fixture.ExpectedTable);

            //Act
            var actualTable = _fixture.MockAnimalCreateTable.Object.CreateTable(_fixture.ListOfAppointments);

            //Assert
            Assert.NotNull(actualTable);
            Assert.Equal(_fixture.ExpectedTable.Rows.Count, actualTable.Rows.Count);
            Assert.Equal(_fixture.ExpectedTable.Columns.Count, actualTable.Columns.Count);
        }

        [Fact]
        public void GetFilledTable_ShouldReturnEmptyTable()
        {
            //Arrange
            _fixture.MockAnimalCreateTable
                .Setup(ser => ser.CreateTable(It.IsAny<PagedList<Appointment>>()))
                .Returns(_fixture.ExpectedEmptyTable);

            //Act
            var actualTable = _fixture.MockAnimalCreateTable.Object.CreateTable(_fixture.EmpyListOfAppointments);

            //Assert
            Assert.NotNull(actualTable);
            Assert.Equal("Medical Card", actualTable.TableName);
            Assert.Equal(_fixture.ExpectedEmptyTable.Rows.Count, actualTable.Rows.Count);
            Assert.Equal(_fixture.ExpectedEmptyTable.Columns.Count, actualTable.Columns.Count);
        }

        [Fact]
        public async Task GetNormalFile_ShouldReturnNormalPdf()
        {
            //Arrange
            _fixture.MockAnimalService
                .Setup(ser => ser.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<AnimalParameters>()))
                .ReturnsAsync(_fixture.ListOfAppointments);

            _fixture.MockAnimalCreateTable
                .Setup(ser => ser.CreateTable(It.IsAny<PagedList<Appointment>>()))
                .Returns(_fixture.ExpectedTable);

            _fixture.MockPDFGenerator
                .Setup(ser => ser.CreatePdf(It.IsAny<DataTable>()))
                .Returns(_fixture.ExpectedPdfFileModel);

            //Act
            var actualResult = await _fixture.MockAnimalMedCardPDFGenerator.GeneratePdf(_fixture.animalParams);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_fixture.ExpectedPdfFileModel.FileStream.Length, actualResult.FileStream.Length);
        }

        [Fact]
        public async Task GetEmptyFile_ShouldReturnEmptyPdf()
        {
            //Arrange
            _fixture.MockAnimalService
                .Setup(ser => ser.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<AnimalParameters>()))
                .ReturnsAsync(_fixture.EmpyListOfAppointments);

            _fixture.MockAnimalCreateTable
                .Setup(ser => ser.CreateTable(It.IsAny<PagedList<Appointment>>()))
                .Returns(_fixture.ExpectedEmptyTable);

            _fixture.MockPDFGenerator
                .Setup(ser => ser.CreatePdf(It.IsAny<DataTable>()))
                .Returns(_fixture.ExpectedPdfFileModelEmpty);

            //Act
            var actualResult = await _fixture.MockAnimalMedCardPDFGenerator.GeneratePdf(_fixture.animalParamsEmpty);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_fixture.ExpectedPdfFileModelEmpty.FileStream.Length, actualResult.FileStream.Length);
        }
    }
}
