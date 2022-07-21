using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace Application.Services
{
    public class PDF_Generator: IPDfGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly string _fileName;
        private readonly string _contentType;

        public PDF_Generator(IConfiguration configuration)
        {
            _configuration = configuration;
            _fileName = _configuration["Pdf:DefaultFileName"];
            _contentType = _configuration["Pdf:ContentType"];
        }

        public async Task<PdfFileModel> CreatePDF(DataTable table)
        {
            //Create a new PDF document
            PdfDocument doc = new PdfDocument();
            //Add a page
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid
            PdfGrid pdfGrid = new PdfGrid();
            //Assign data source
            pdfGrid.DataSource = table;
            //Draw grid to the page of PDF document
            pdfGrid.Draw(page, new PointF(10, 10));
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
