using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Services;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet()]
        public async Task<ActionResult> GetSpecializations()
        {
            return Ok(_mapper.Map<IEnumerable<SpecializationViewModel>>(await _service.GetAllSpecializationsAsync()));
        }

        [HttpGet("api/specialization/{id:int:min(1)}")]
        public async Task<ActionResult> GetSpecializationById([FromRoute] int id)
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


        [HttpPut("api/specialization/{id:int:min(1)}")]
        public async Task<ActionResult> UpdateSpecialization([FromRoute]int id, SpecializationViewModel updated)
        {
            if(!ModelState.IsValid)
                throw new BadRequestException("invalid parameters");
            await _service.UpdateSpecializationAsync(id, _mapper.Map<Specialization>(updated));
            return Ok();
        }

        [HttpDelete("api/specialization/{id:int:min(1)}")]
        public async Task<ActionResult> DeleteSpecialization([FromRoute] int id)
        {
            await _service.DeleteSpecializationAsync(id);
            return Ok();
        }
    }
}
