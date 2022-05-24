﻿using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels.ProcedureViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers;

[Route("api/Procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IMapper _mapper;

    public ProcedureController(IProcedureService procedureService, IMapper mapper)
    {
        _procedureService = procedureService;
        _mapper = mapper;
    }

    [HttpGet("/Procedures/getall")]
    public async Task<ActionResult<IEnumerable<ProcedureViewModelBase>>> GetAll()
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

        return Ok(_mapper.Map<IEnumerable<Procedure>, IEnumerable<ProcedureViewModelBase>>(result));
    }
    
    [HttpGet("/Procedures/get/{id}")]
    public async Task<ActionResult<ProcedureViewModelBase>> GetById(int id)
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

        return Ok(_mapper.Map<Procedure, ProcedureViewModelBase>(result));
    }
    
    [HttpPost("/Procedures/new")]
    public async Task<ActionResult> Create(ProcedureViewModel procedure)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        await _procedureService.CreateNewProcedureAsync(_mapper.Map<ProcedureViewModel, Procedure>(procedure));
        return Ok();
    }
    
    [HttpDelete("/Procedures/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return Ok();
    }
    
    [HttpPut("/Procedures/update/{id:int}")]
    public async Task<ActionResult> Update(int id, ProcedureViewModel newProcedure)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        await _procedureService.UpdateProcedureAsync(id, _mapper.Map<ProcedureViewModel, Procedure>(newProcedure));
        return Ok();
    }
}