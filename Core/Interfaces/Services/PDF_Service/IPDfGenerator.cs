using Core.Models;
using System.Data;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface IPDfGenerator
    {
        PdfFileModel CreatePDF(DataTable table);
    }
}
