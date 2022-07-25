using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.AnimalViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;
using Core.ViewModels;
using Core.Paginator;
using Core.Paginator.Parameters;
using WebApi.Extensions;
using Core.Interfaces.Services.PDF_Service;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IViewModelMapperUpdater<AnimalViewModel, Animal> _mapperVMtoM;
        private readonly IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>> _mapperAnimalListToList;
        private readonly IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>> _pagedMedCardMapper;
        private readonly IGenerateFullPdf<AnimalParameters> _generatePDF;


        public AnimalController(
            IAnimalService animalService,
            IViewModelMapperUpdater<AnimalViewModel, Animal> mapperVMtoM,
            IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>> mapperAnimalListToList,
            IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>> pagedMedCardMapper,
            IGenerateFullPdf<AnimalParameters> generatePDF)
        {
            _animalService = animalService;
            _mapperVMtoM = mapperVMtoM;
            _mapperAnimalListToList = mapperAnimalListToList;
            _pagedMedCardMapper = pagedMedCardMapper;
            _generatePDF = generatePDF;
        }

        [HttpGet("{ownerId:int:min(1)}")]
        public async Task<IEnumerable<AnimalViewModel>> GetAsync([FromRoute] int ownerId)
        {
            var animals = await _animalService.GetAsync(ownerId);
            var map = _mapperAnimalListToList.Map(animals);
            return map;
        }

        [HttpGet("medcard")]
        public async Task<PagedReadViewModel<AnimalMedCardViewModel>> GetMedCardAsync([FromQuery] AnimalParameters animalParameters)
        {
            var appointments = await _animalService.GetAllAppointmentsWithAnimalIdAsync(animalParameters);
            var map = _pagedMedCardMapper.Map(appointments);
            return map;
        }

        [HttpGet("generatePDF")]
        public async Task<FileStreamResult> GeneratePDF([FromQuery] AnimalParameters animalParameters)
        {
            var pdfFileParams = await _generatePDF.GeneratePdf(animalParameters);
            var result = this.File(pdfFileParams);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody]AnimalViewModel model)
        {
            var map = _mapperVMtoM.Map(model);
            await _animalService.CreateAsync(map);
            return Ok();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute]int id)
        {
            await _animalService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody]AnimalViewModel model)
        {
            var prevAnimal = await _animalService.GetByIdAsync(model.Id);
            _mapperVMtoM.Map(model, prevAnimal);
            await _animalService.UpdateAsync(prevAnimal);
            return NoContent();
        }
    }
}
