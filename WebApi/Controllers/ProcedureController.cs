using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[Route("api/procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IViewModelMapper<ProcedureViewModelBase, Procedure> _procedureMapper;
    private readonly IViewModelMapper<Procedure, ProcedureReadViewModel> _procedureViewModelMapper;

    private readonly IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>
        _procedureEnumerableViewModelMapper;

    public ProcedureController(IProcedureService procedureService, 
        IViewModelMapper<ProcedureViewModelBase, Procedure> procedureMapper,
        IViewModelMapper<Procedure, ProcedureReadViewModel> procedureViewModelMapper, 
        IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>> procedureEnumerableViewModelMapper)
    {
        _procedureService = procedureService;
        _procedureMapper = procedureMapper;
        _procedureViewModelMapper = procedureViewModelMapper;
        _procedureEnumerableViewModelMapper = procedureEnumerableViewModelMapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcedureReadViewModel>>> GetAsync()
    {
        var procedures = await _procedureService.GetAllProceduresAsync();
        var viewModels = _procedureEnumerableViewModelMapper.Map(procedures);
        return Ok(viewModels);
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ProcedureReadViewModel>> GetAsync([FromRoute]int id)
    {
        var result = await _procedureService.GetByIdAsync(id);
        var viewModel = _procedureViewModelMapper.Map(result);
        return Ok(viewModel);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody]ProcedureViewModelBase procedure)
    { 
        await _procedureService.CreateNewProcedureAsync(_procedureMapper.Map(procedure));

        return Ok();
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromBody]ProcedureViewModelBase newProcedure)
    {
        await _procedureService.UpdateProcedureAsync(_procedureMapper.Map(newProcedure));
        return NoContent();
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateProcedureSpecializationsAsync(
        [FromRoute]int id, 
        [FromBody]IEnumerable<int> specializationIds)
    { 
        await _procedureService.UpdateProcedureSpecializationsAsync(id, specializationIds);
        return NoContent();
    }
}