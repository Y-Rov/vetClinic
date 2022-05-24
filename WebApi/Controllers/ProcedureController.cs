using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;
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

    [HttpGet("/Procedures/getall")]
    public async Task<ActionResult<IEnumerable<ProcedureViewModel>>> GetAll()
    {
        IEnumerable<Procedure> result;
        try
        {
            result = await _procedureService.GetAllProceduresAsync();
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<IEnumerable<Procedure>, IEnumerable<ProcedureViewModel>>(result));
    }
    
    [HttpGet("/Procedures/get/{id}")]
    public async Task<ActionResult<ProcedureViewModel>> GetById(int id)
    {
        Procedure result;
        try
        {
            result = await _procedureService.GetByIdAsync(id);
        }
        catch (NullReferenceException)
        {
            throw new NotFoundException();
        }        
        catch (InvalidOperationException)
        {
            throw new NotFoundException();
        }

        return Ok(_mapper.Map<Procedure, ProcedureViewModel>(result));
    }
    
    [HttpPost("/Procedures/new")]
    public async Task<ActionResult> Create(ProcedureViewModelBase procedure)
    {
        var validationResult = await _validator.ValidateAsync(procedure);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors);
        }

        var result =
            await _procedureService.CreateNewProcedureAsync(_mapper.Map<ProcedureViewModelBase, Procedure>(procedure));
        
        return Created(nameof(Create), result);
    }
    
    [HttpDelete("/Procedures/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return NoContent();
    }
    
    [HttpPut("/Procedures/update/{id:int}")]
    public async Task<ActionResult> Update(int id, ProcedureViewModelBase newProcedure)
    {
        var validationResult = await _validator.ValidateAsync(newProcedure);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors);
        }
        
        await _procedureService.UpdateProcedureAsync(id, _mapper.Map<ProcedureViewModelBase, Procedure>(newProcedure));
        return NoContent();
    }
}