using Core.Models;
using System.Data;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface IPdfGenerator
    {
        PdfFileModel CreatePdf(DataTable table);
    }
}
