using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.AnimalViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;
using Core.ViewModels.AppointmentsViewModel;
using Core.ViewModels;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;
        private readonly IViewModelMapper<AnimalViewModel, Animal> _mapperVMtoM;
        private readonly IViewModelMapper<Animal, AnimalViewModel> _mapperMtoVM;
        private readonly IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>> _mapperAnimalListToList;
        private readonly IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>> _mapperMedCard;

        public AnimalController(
            IAnimalService animalService,
            IViewModelMapper<AnimalViewModel, Animal> mapperVMtoM,
            IViewModelMapper<Animal, AnimalViewModel> mapperMtoVM,
            IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>> mapperAnimalListToList,
            IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>> mapperMedCard)
        {
            _animalService = animalService;
            _mapperVMtoM = mapperVMtoM;
            _mapperMtoVM = mapperMtoVM;
            _mapperAnimalListToList = mapperAnimalListToList;
            _mapperMedCard = mapperMedCard;
        }

        [HttpGet]
        public async Task<IEnumerable<AnimalViewModel>> GetAsync()
        {
            var animals = await _animalService.GetAsync();
            var map = _mapperAnimalListToList.Map(animals);
            return map;
        }

        [HttpGet("medcard/{id:int:min(1)}")]
        public async Task<IEnumerable<AppointmentReadViewModel>> GetMedCardAsync([FromRoute] int id)
        {
            var appointments = await _animalService.GetAllAppointmentsWithAnimalIdAsync(id);
            var map = _mapperMedCard.Map(appointments);
            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<AnimalViewModel> GetAsync([FromRoute]int id)
        {
            var animal = await _animalService.GetByIdAsync(id);
            var map = _mapperMtoVM.Map(animal);
            return map;
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
            var map = _mapperVMtoM.Map(model);
            await _animalService.UpdateAsync(map);
            return NoContent();
        }
    }
}
