using Core.Models;
using System.Data;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface IPDfGenerator
    {
        Task<PdfFileModel> CreatePDF(DataTable table);
    }
}
