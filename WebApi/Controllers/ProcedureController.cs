using System.Net;
using Application.Services;
using AutoMapper;
using Core.ViewModel.ProcedureDTOs;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
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
    private readonly ProcedureDtoValidator _validator;

    public ProcedureController(IProcedureService procedureService, IMapper mapper, ProcedureDtoValidator validator)
    {
        _procedureService = procedureService;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet("/Procedures/getall")]
    public async Task<ActionResult<IEnumerable<ProcedureModel>>> GetAll()
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

        return Ok(_mapper.Map<IEnumerable<Procedure>, IEnumerable<ProcedureModel>>(result));
    }
    
    [HttpGet("/Procedures/get/{id}")]
    public async Task<ActionResult<ProcedureModel>> GetById(int id)
    {
        Procedure result;
        try
        {
            result = await _procedureService.GetByIdAsync(id);
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }        
        catch (InvalidOperationException)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<Procedure, ProcedureModel>(result));
    }
    
    [HttpPost("/Procedures/new")]
    public async Task<ActionResult> Create(ProcedureDTO procedure)
    {
        var validationResult = await _validator.ValidateAsync(procedure);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var createdProcedure = await _procedureService.CreateNewProcedureAsync(_mapper.Map<ProcedureDTO, Procedure>(procedure));
        return Created(nameof(Create), _mapper.Map<ProcedureModel>(createdProcedure));
    }
    
    [HttpDelete("/Procedures/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return NoContent();
    }
    
    [HttpPut("/Procedures/update/{id:int}")]
    public async Task<ActionResult> Update(int id, ProcedureDTO newProcedure)
    {
        var validationResult = await _validator.ValidateAsync(newProcedure);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        await _procedureService.UpdateProcedureAsync(id, _mapper.Map<ProcedureDTO, Procedure>(newProcedure));
        return NoContent();
    }
}