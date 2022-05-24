using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers
{
    [Route("api/specialization")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        ISpecializationService _service;
        IMapper _mapper;

        public SpecializationController(ISpecializationService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("/getSpecializations")]
        public async Task<ActionResult> GetSpecializations()
        {
            return Ok(_mapper.Map<IEnumerable<SpecializationViewModel>>(await _service.GetAllSpecializationsAsync()));
        }

        [HttpGet("/getSpecializationById/{id}")]
        public async Task<ActionResult> GetSpecializationById(int id)
        {
            return Ok(_mapper.Map<SpecializationViewModel>(await _service.GetSpecializationByIdAsync(id)));
        }

        [HttpPost]
        public async Task<ActionResult> AddSpecialization(SpecializationViewModel specialization)
        {
            return !ModelState.IsValid ? throw new BadRequestException() :
                Ok(_mapper.Map<SpecializationViewModel>(
                    await _service.AddSpecializationAsync(_mapper.Map<Specialization>(specialization))));
        }


        [HttpPut("/update/{id}")]
        public async Task<ActionResult> UpdateSpecialization(int id, SpecializationViewModel updated)
        {
            return !ModelState.IsValid ? throw new BadRequestException() :
                Ok(await _service.UpdateSpecializationAsync(id, _mapper.Map<Specialization>(updated)));
        }

        [HttpDelete("/delete/{id}")]
        public async Task<ActionResult> DeleteSpecialization(int id)
        {
            return Ok(await _service.DeleteSpecializationAsync(id));
        }
    }
}
