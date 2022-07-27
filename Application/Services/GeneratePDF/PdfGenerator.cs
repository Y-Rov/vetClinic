using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace Application.Services.GeneratePDF
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly string _fileName;
        private readonly string _contentType;

        public PdfGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
            _fileName = _configuration["Pdf:DefaultFileName"];
            _contentType = _configuration["Pdf:ContentType"];
        }

        public PdfFileModel CreatePdf(DataTable table)
        {
            //Create a new PDF document
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
                ContentType = _contentType,
                DefaultFileName = _fileName,
                FileStream = stream
            };

            return pdfFileParam;
        }
    }
}
