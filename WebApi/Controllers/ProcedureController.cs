using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;
using WebApi.Validators;

namespace WebApi.Controllers;

[Route("api/Procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly ProcedureViewModelBaseValidator _validator;
    private readonly IViewModelMapper<ProcedureViewModelBase, Procedure> _procedureMapper;
    private readonly IViewModelMapperAsync<Procedure, ProcedureSpecViewModel> _procedureViewModelMapper;

    public ProcedureController(IProcedureService procedureService, 
        ProcedureViewModelBaseValidator validator,
        IViewModelMapper<ProcedureViewModelBase, Procedure> procedureMapper,
        IViewModelMapperAsync<Procedure, ProcedureSpecViewModel> procedureViewModelMapper)
    {
        _procedureService = procedureService;
        _validator = validator;
        _procedureMapper = procedureMapper;
        _procedureViewModelMapper = procedureViewModelMapper;
    }

    [HttpGet("/api/Procedures/")]
    public async Task<ActionResult<IEnumerable<ProcedureSpecViewModel>>> GetAsync()
    {
        var procedures = await _procedureService.GetAllProceduresAsync();
        var viewModels = new List<ProcedureSpecViewModel>();
        foreach (var p in procedures)
        {
            viewModels.Add(await _procedureViewModelMapper.MapAsync(p));
        }
        return Ok(viewModels);
    }
    
    [HttpGet("/api/Procedures/{id:int:min(1)}")]
    public async Task<ActionResult<ProcedureSpecViewModel>> GetAsync([FromRoute]int id)
    {
        var result = await _procedureService.GetByIdAsync(id);
        return Ok(await _procedureViewModelMapper.MapAsync(result));
    }
    
    [HttpPost("/api/Procedures/")]
    public async Task<ActionResult> CreateAsync(ProcedureViewModelBase procedure)
    {
        var validationResult = await _validator.ValidateAsync(procedure);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors.ToString());
        }

        var result =
            await _procedureService.CreateNewProcedureAsync(_procedureMapper.Map(procedure));
        
        return Created(nameof(CreateAsync), result);
    }
    
    [HttpDelete("/api/Procedures/{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return NoContent();
    }
    
    [HttpPut("/api/Procedures/")]
    public async Task<ActionResult> UpdateAsync(ProcedureViewModelBase newProcedure)
    {
        var validationResult = await _validator.ValidateAsync(newProcedure);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors.ToString());
        }
        
        await _procedureService.UpdateProcedureAsync(_procedureMapper.Map(newProcedure));
        return NoContent();
    }

    [HttpPatch("/api/Procedures/{id:int:min(1)}")]
    public async Task<ActionResult> UpdateProcedureSpecializationsAsync([FromRoute]int id, IEnumerable<int> specializationIds)
    { 
        await _procedureService.UpdateProcedureSpecializationsAsync(id, specializationIds);
        return NoContent();
    }
}