using Core.Entities;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Paginator.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AnimalMedCard_PDF
{
    public class AnimalMedCardPDFGenerator : IGenerateFullPDF<AnimalParameters>
    {
        private readonly IAnimalService _animalService;
        private readonly ICreateTableForPDF<Appointment> _createTable;
        private readonly IPDfGenerator _PDFGenerator;

        public AnimalMedCardPDFGenerator(
            IAnimalService animalService,
            ICreateTableForPDF<Appointment> createTable,
            IPDfGenerator pDFGenerator)
        {
            _animalService = animalService;
            _createTable = createTable;
            _PDFGenerator = pDFGenerator;
        }

        public async Task<PdfFileModel> GeneratePDF(AnimalParameters parameters)
        {
            var medCardList = await _animalService.GetAllAppointmentsWithAnimalIdAsync(parameters);
            var medCardTable = _createTable.CreateTable(medCardList);
            var pdfFileParams = _PDFGenerator.CreatePDF(medCardTable);

            return pdfFileParams;

        }
    }
}
