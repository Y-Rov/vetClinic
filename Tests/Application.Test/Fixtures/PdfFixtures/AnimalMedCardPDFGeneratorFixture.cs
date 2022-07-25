using Application.Services.GeneratePDF;
using Application.Services.GeneratePDF.AnimalMedCard_PDF;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Fixtures.PdfFixtures
{
    public class AnimalMedCardPdfGeneratorFixture
    {
        public AnimalMedCardPdfGeneratorFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            ListOfAppointments = GetListOfAppointments();
            EmpyListOfAppointments = GetEmptyPagedMedCard();
            ExpectedTable = GetTable(ListOfAppointments);
            ExpectedEmptyTable = GetEmptyTable();
            ExpectedPdfFileModel = GetFileStream(ExpectedTable);
            ExpectedPdfFileModelEmpty = GetFileStream(ExpectedEmptyTable);


            MockAnimalService = fixture.Freeze<Mock<IAnimalService>>();
            MockAnimalCreateTable = fixture.Freeze<Mock<ICreateTableForPdf<Appointment>>>();
            MockPDFGenerator = fixture.Freeze<Mock<IPdfGenerator>>();

            animalParams = new()
            {
                animalId = 13,
                PageNumber = 1,
                PageSize = 0
            };

            animalParamsEmpty = new()
            {
                animalId = 0,
                PageNumber = 1,
                PageSize = 0
            };


            MockAnimalMedCardPDFGenerator = new AnimalMedCardPdfGenerator(
                MockAnimalService.Object,
                MockAnimalCreateTable.Object,
                MockPDFGenerator.Object);
        }

        public AnimalMedCardPdfGenerator MockAnimalMedCardPDFGenerator { get; }
        public DataTable ExpectedTable { get; }
        public DataTable ExpectedEmptyTable { get; }
        public PdfFileModel ExpectedPdfFileModel { get; }
        public PdfFileModel ExpectedPdfFileModelEmpty { get; }
        public Mock<IAnimalService> MockAnimalService { get; }
        public Mock<ICreateTableForPdf<Appointment>> MockAnimalCreateTable { get; }
        public Mock<IPdfGenerator> MockPDFGenerator { get; }
        public PagedList<Appointment> ListOfAppointments { get; }
        public PagedList<Appointment> EmpyListOfAppointments { get; }
        public AnimalParameters animalParams { get; }
        public AnimalParameters animalParamsEmpty { get; }

        private DataTable GetTable(PagedList<Appointment> listOfAppointments)
        {
            var table = new DataTable("Medical Card");

            table.Columns.Add("Data");
            table.Columns.Add("Decese");

            foreach (var x in listOfAppointments)
            {
                table.Rows.Add(new object[] { $"{x.Date}", $"{x.Disease}" });
            }

            return table;
        }

        private DataTable GetEmptyTable()
        {
            var table = new DataTable("Medical Card");

            table.Columns.Add("Data");
            table.Columns.Add("Decese");

            return table;
        }

        private PagedList<Appointment> GetListOfAppointments()
        {
            var appointments = GetAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private PagedList<Appointment> GetEmptyPagedMedCard()
        {
            var appointments = GetEmptyAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private List<Appointment> GetAppointmentList()
        {
            var appointments = new List<Appointment>()
            {

                new Appointment()
                {
                    Id = 1,
                    AnimalId = 1,
                    Date = new DateTime(2022, 06, 20),
                    Disease = "string",
                    MeetHasOccureding = true
                },
                new Appointment()
                {
                    Id = 2,
                    AnimalId = 2,
                    Date = new DateTime(2022, 06, 21),
                    Disease = "string",
                    MeetHasOccureding = true
                }
            };

            return appointments;
        }

        private List<Appointment> GetEmptyAppointmentList()
        {
            return new List<Appointment>();
        }

        private PdfFileModel GetFileStream(DataTable table)
        {
            PdfDocument doc = new PdfDocument();
            //Add a page
            PdfPage page = doc.Pages.Add();

            //Table Name
            PdfGraphics graphics = page.Graphics;

            graphics.DrawString($"{table.TableName}",
                font: PdfParams.font,
                brush: PdfParams.color,
                new PointF(10, 3));

            //Create a PdfGrid
            PdfGrid pdfGrid = new PdfGrid();
            //Assign data source
            pdfGrid.DataSource = table;

            //Adding style
            PdfGridBuiltinStyleSettings tableStyleOption = new PdfGridBuiltinStyleSettings();
            tableStyleOption.ApplyStyleForBandedRows = true;
            tableStyleOption.ApplyStyleForHeaderRow = true;

            //Apply built-in table style
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent4, tableStyleOption);

            //Draw grid to the page of PDF document
            pdfGrid.Draw(page, new PointF(10, 40));
            //Save the PDF document to stream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            doc.Close(true);
            //Creates a FileContentResult object by using the file contents, content type, and file name.

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
