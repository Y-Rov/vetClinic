using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validators;

namespace WebApi.Controllers;

[Route("api/Procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IMapper _mapper;
    private readonly ProcedureViewModelBaseValidator _validator;

    public ProcedureController(IProcedureService procedureService, IMapper mapper, ProcedureViewModelBaseValidator validator)
    {
        _procedureService = procedureService;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet("/api/Procedures/")]
    public async Task<ActionResult<IEnumerable<ProcedureViewModel>>> GetAsync()
    {
        var result = await _procedureService.GetAllProceduresAsync();

        return Ok(_mapper.Map<IEnumerable<Procedure>, IEnumerable<ProcedureViewModel>>(result));
    }
    
    [HttpGet("/api/Procedures/{id:int:min(1)}")]
    public async Task<ActionResult<ProcedureViewModel>> GetAsync([FromRoute]int id)
    {
        var result = await _procedureService.GetByIdAsync(id);
        return Ok(_mapper.Map<Procedure, ProcedureViewModel>(result));
    }
    
    [HttpPost("/api/Procedures/")]
    public async Task<ActionResult> CreateAsync(ProcedureViewModelBase procedure)
    {
        var validationResult = await _validator.ValidateAsync(procedure);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors);
        }

        var result =
            await _procedureService.CreateNewProcedureAsync(_mapper.Map<ProcedureViewModelBase, Procedure>(procedure));
        
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
            throw new BadRequestException(validationResult.Errors);
        }
        
        await _procedureService.UpdateProcedureAsync(_mapper.Map<ProcedureViewModelBase, Procedure>(newProcedure));
        return NoContent();
    }

    [HttpPatch("/api/Procedures/{id:int:min(1)}")]
    public async Task<ActionResult> UpdateProcedureSpecializationsAsync([FromRoute]int id, IEnumerable<int> specializationIds)
    {
        try
        {
            await _procedureService.UpdateProcedureSpecializationsAsync(id, specializationIds);
        }
        catch (InvalidOperationException)
        {
            throw new BadRequestException();
        }
        return NoContent();
    }
}