using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interfaces.Services.PDF_Service
{
    public interface IGenerateFullPdf<TypeOfParameters>
    {
        Task<PdfFileModel> GeneratePdf(TypeOfParameters parameters);
    }
}
