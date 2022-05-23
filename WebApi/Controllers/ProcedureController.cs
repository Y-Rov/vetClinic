using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels;
using DataAccess.Configurations;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interfaces;
using WebApi.Exceptions;

namespace WebApi.Controllers;

[Route("api/Procedures")]
[ApiController]
public class ProcedureController : ControllerBase
{
    private readonly IProcedureService _procedureService;
    private readonly IViewModelMapper<ProcedureViewModelBase, Procedure> _procedureMapper;
    private readonly IViewModelMapper<Procedure, ProcedureWithSpecializationsViewModel> _modelMapper;

    public ProcedureController(IProcedureService procedureService, 
        IViewModelMapper<ProcedureViewModelBase, Procedure> procedureMapper,
        IViewModelMapper<Procedure, ProcedureWithSpecializationsViewModel> modelMapper)
    {
        _procedureService = procedureService;
        _procedureMapper = procedureMapper;
        _modelMapper = modelMapper;
    }

    [HttpGet("/Procedures/getall")]
    public async Task<ActionResult<IEnumerable<ProcedureWithSpecializationsViewModel>>> GetAll()
    {
        IEnumerable<Procedure> procedures;
        try
        {
            procedures = await _procedureService.GetAllProceduresAsync();
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }

        var result = new List<ProcedureWithSpecializationsViewModel>();
        foreach (var p in procedures)
        {
            result.Add(_modelMapper.Map(p));
        }

        return Ok(result);
    }
    
    [HttpGet("/Procedures/get/{id}")]
    public async Task<ActionResult<ProcedureWithSpecializationsViewModel>> GetById(int id)
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

        return Ok(_modelMapper.Map(result));
    }
    
    [HttpPost("/Procedures/new")]
    public async Task<ActionResult> Create(ProcedureViewModelBase procedure)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        await _procedureService.CreateNewProcedureAsync(_procedureMapper.Map(procedure));
        return Ok();
    }
    
    [HttpDelete("/Procedures/delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _procedureService.DeleteProcedureAsync(id);
        return Ok();
    }
    
    [HttpPut("/Procedures/update/{id:int}")]
    public async Task<ActionResult> Update(int id, ProcedureViewModelBase newProcedure)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        await _procedureService.UpdateProcedureAsync(id, _procedureMapper.Map(newProcedure));
        return Ok();
    }
}