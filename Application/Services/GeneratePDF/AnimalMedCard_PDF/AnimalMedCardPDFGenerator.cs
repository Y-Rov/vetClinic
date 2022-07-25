using Core.Entities;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Paginator.Parameters;

namespace Application.Services.GeneratePDF.AnimalMedCard_PDF
{
    public class AnimalMedCardPdfGenerator : IGenerateFullPdf<AnimalParameters>
    {
        private readonly IAnimalService _animalService;
        private readonly ICreateTableForPdf<Appointment> _createTable;
        private readonly IPdfGenerator _PDFGenerator;

        public AnimalMedCardPdfGenerator(
            IAnimalService animalService,
            ICreateTableForPdf<Appointment> createTable,
            IPdfGenerator pDFGenerator)
        {
            _animalService = animalService;
            _createTable = createTable;
            _PDFGenerator = pDFGenerator;
        }

        public async Task<PdfFileModel> GeneratePdf(AnimalParameters parameters)
        {
            var medCardList = await _animalService.GetAllAppointmentsWithAnimalIdAsync(parameters);
            var medCardTable = _createTable.CreateTable(medCardList);
            var pdfFileParams = _PDFGenerator.CreatePdf(medCardTable);

            return pdfFileParams;

        }
    }
}
