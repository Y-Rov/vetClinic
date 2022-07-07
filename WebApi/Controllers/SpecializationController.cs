using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/specialization")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        readonly ISpecializationService _service;
        readonly IViewModelMapper<SpecializationViewModel, Specialization> _mapper;
        readonly IViewModelMapper<Specialization, SpecializationViewModel> _viewModelMapper;
        readonly IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>
            _procedureEnumerableViewModelMapper;
        readonly IViewModelMapper<PagedList<Specialization>, PagedReadViewModel<SpecializationViewModel>> _pagedMapper;

        public SpecializationController(
            ISpecializationService service, 
            IViewModelMapper<SpecializationViewModel, Specialization> mapper, 
            IViewModelMapper<Specialization, SpecializationViewModel> viewModelMapper, 
            IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> procedureEnumerableViewModelMapper,
            IViewModelMapper<PagedList<Specialization>, PagedReadViewModel<SpecializationViewModel>> pagedMapper)
        {
            _service = service;
            _mapper = mapper;
            _viewModelMapper = viewModelMapper;
            _procedureEnumerableViewModelMapper = procedureEnumerableViewModelMapper;
            _pagedMapper = pagedMapper;
        }

        [HttpGet]
        public async Task<PagedReadViewModel<SpecializationViewModel>> GetSpecializations(
            [FromQuery]SpecializationParameters specializationParameters)
        {
            var specializations = 
                await _service.GetAllSpecializationsAsync(specializationParameters);

            var mappedSpecializations = _pagedMapper.Map(specializations);

            return mappedSpecializations;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<SpecializationViewModel> GetSpecializationById([FromRoute] int id)
        {
            var specialization = await _service.GetSpecializationByIdAsync(id);
            var mappedSpecialization = _viewModelMapper.Map(specialization);
            return mappedSpecialization;
        }

        [HttpGet("{id:int:min(1)}/procedures")]
        public async Task<IEnumerable<ProcedureReadViewModel>> GetSpecializationProcedures([FromRoute] int id)
        {
            var procedures = await _service.GetSpecializationProcedures(id);
            var mappedProcedures = _procedureEnumerableViewModelMapper.Map(procedures);
            return mappedProcedures;
        }

        [HttpPost]
        public async Task<Specialization> AddSpecialization([FromBody]SpecializationViewModel specialization)
        {
            var specializationRaw = _mapper.Map(specialization);
            var result = await _service.AddSpecializationAsync(specializationRaw);
            return result;
        }

        [HttpPut("addProcedure/{specId:int:min(1)}/{procId:int:min(1)}")]
        public async Task<IActionResult> AddProcedureToSpecialization([FromRoute] int specId, [FromRoute] int procId)
        {
            await _service.AddProcedureToSpecialization(specId, procId);
            return NoContent();
        }

        [HttpPut("removeProcedure/{specId:int:min(1)}/{procId:int:min(1)}")]
        public async Task<IActionResult> RemoveProcedureFromSpecialization([FromRoute] int specId, [FromRoute] int procId)
        {
            await _service.RemoveProcedureFromSpecialization(specId, procId);
            return NoContent();
        }

        [HttpPut("addUser/{specId:int:min(1)}/{userId:int:min(1)}")]
        public async Task<IActionResult> AddUserToSpecialization([FromRoute] int specId, [FromRoute] int userId)
        {
            await _service.AddUserToSpecialization(specId, userId);
            return NoContent();
        }

        [HttpPut("removeUser/{specId:int:min(1)}/{userId:int:min(1)}")]
        public async Task<IActionResult> DeleteUserFromSpecialization([FromRoute] int specId, [FromRoute] int userId)
        {
            await _service.RemoveUserFromSpecialization(specId, userId);
            return NoContent();
        }

        [HttpPut("addProcedures/{id:int:min(1)}")]
        public async Task<ActionResult> AddProceduresToSpecialization(
            [FromRoute]int id, 
            [FromBody]SpecializationUpdateViewModel specialization)
        {
            await _service.UpdateSpecializationProceduresAsync(id, specialization.ProcedureIds);
            return NoContent();
        }

        [HttpPut("addUsers/{id:int:min(1)}")]
        public async Task<ActionResult> AddUsersToSpecialization(
            [FromRoute]int id,
            [FromBody]SpecializationUpdateViewModel specialization)
        {
            await _service.UpdateSpecializationUsersAsync(id, specialization.UsersIds);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]   
        public async Task<ActionResult> UpdateSpecialization([FromRoute]int id, [FromBody]SpecializationViewModel updated)
        {
            await _service.UpdateSpecializationAsync(id,_mapper.Map(updated));
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteSpecialization([FromRoute] int id)
        {
            await _service.DeleteSpecializationAsync(id);
            return NoContent();
        }
    }
}
