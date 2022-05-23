using AutoMapper;
using Core.DTO;
using Core.Entities;
using Core.Interfaces.Services;
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
            IEnumerable<Specialization> specializations;
            try
            {
                specializations = await _service.GetAllSpecializationsAsync();
            }
            catch(NullReferenceException)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<SpecializationDTO>>(specializations));
        }

        [HttpGet("/getSpecializationById/{id}")]
        public async Task<ActionResult> GetSpecializationById(int id)
        {
            Specialization specialization;
            try
            {
                specialization = await _service.GetSpecializationByIdAsync(id);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<SpecializationDTO>(specialization));
        }

        [HttpPost]
        public async Task<ActionResult> AddSpecialization(SpecializationDTO specialization) =>
            !ModelState.IsValid ? throw new BadRequestException() :
                Ok(_mapper.Map<SpecializationDTO>(
                    await _service.AddSpecializationAsync(_mapper.Map<Specialization>(specialization))));

        
        [HttpPut("/update/{id}")]
        public async Task<ActionResult> UpdateSpecialization(int id, SpecializationDTO updated) =>
            !ModelState.IsValid? throw new BadRequestException() :
                Ok(await _service.UpdateSpecializationAsync(id,_mapper.Map<Specialization>(updated)));

        [HttpDelete("/delete/{id}")]
        public async Task<ActionResult> DeleteSpecialization(int id) =>
            Ok(await _service.DeleteSpecializationAsync(id));
    }
}
