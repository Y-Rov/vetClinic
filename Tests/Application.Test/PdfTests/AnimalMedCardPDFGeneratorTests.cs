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
