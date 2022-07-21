using Core.Models;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface IGenerateFullPDF<TypeOfParameters>
    {
        Task<PdfFileModel> GeneratePDF(TypeOfParameters parameters);
    }
}
