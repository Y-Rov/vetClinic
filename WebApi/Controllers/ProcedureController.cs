using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;
using WebApi.Validators;

namespace WebApi.Controllers;

[Route("api/procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IViewModelMapper<ProcedureViewModelBase, Procedure> _procedureMapper;
    private readonly IViewModelMapper<Procedure, ProcedureSpecViewModel> _procedureViewModelMapper;

    public ProcedureController(IProcedureService procedureService, 
        IViewModelMapper<ProcedureViewModelBase, Procedure> procedureMapper,
        IViewModelMapper<Procedure, ProcedureSpecViewModel> procedureViewModelMapper)
    {
        _procedureService = procedureService;
        _procedureMapper = procedureMapper;
        _procedureViewModelMapper = procedureViewModelMapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProcedureSpecViewModel>>> GetAsync()
    {
        var procedures = await _procedureService.GetAllProceduresAsync();
        var viewModels = new List<ProcedureSpecViewModel>();
        foreach (var p in procedures)
        {
            viewModels.Add(_procedureViewModelMapper.Map(p));
        }
        return Ok(viewModels);
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ProcedureSpecViewModel>> GetAsync([FromRoute]int id)
    {
        var result = await _procedureService.GetByIdAsync(id);
        return Ok(_procedureViewModelMapper.Map(result));
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateAsync(ProcedureViewModelBase procedure)
    {
        var result =
            await _procedureService.CreateNewProcedureAsync(_procedureMapper.Map(procedure));
        
        return Created(nameof(GetAsync), result);
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return NoContent();
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateAsync(ProcedureViewModelBase newProcedure)
    {
        await _procedureService.UpdateProcedureAsync(_procedureMapper.Map(newProcedure));
        return NoContent();
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateProcedureSpecializationsAsync([FromRoute]int id, IEnumerable<int> specializationIds)
    { 
        await _procedureService.UpdateProcedureSpecializationsAsync(id, specializationIds);
        return NoContent();
    }
}