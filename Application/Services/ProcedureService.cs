﻿using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Newtonsoft.Json;

namespace Application.Services;

public class ProcedureService : IProcedureService
{
    private readonly IProcedureRepository _procedureRepository;
    private readonly ILoggerManager _loggerManager;

    public ProcedureService(
        IProcedureRepository procedureRepository,
        ILoggerManager loggerManager)
    {
        _procedureRepository = procedureRepository;
        _loggerManager = loggerManager;
    }

    public async Task<Procedure> CreateNewProcedureAsync(Procedure procedure)    
    {
        var result = await _procedureRepository.AddProcedureAsync(procedure);
        await _procedureRepository.SaveChangesAsync();
        return result;
    }

    public async Task UpdateProcedureAsync(Procedure newProcedure)   
    {
        var oldProcedure = await _procedureRepository.GetProcedureByIdAsync(newProcedure.Id);
        if (oldProcedure is null)
        {
            _loggerManager.LogWarn($"Procedure with id {newProcedure.Id} does not exist");
            throw new NotFoundException($"Procedure with Id {newProcedure.Id} does not exist");
        }

        _loggerManager.LogInfo($"Updating procedure with id {newProcedure.Id}...");
        await _procedureRepository.UpdateProcedureAsync(newProcedure);
        await _procedureRepository.SaveChangesAsync();
    }

    public async Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds)
    {
        var procedureToUpdate = await _procedureRepository.GetProcedureByIdAsync(procedureId);
        if (procedureToUpdate is null)
        {
            _loggerManager.LogWarn($"Procedure with id {procedureId} does not exist");
            throw new NotFoundException($"Procedure with Id {procedureId} does not exist");
        }
        try
        {
            _loggerManager.LogInfo($"Updating specializations list of the procedure with Id {procedureId}...");
            await _procedureRepository.UpdateProcedureSpecializationsAsync(procedureToUpdate, specializationIds);
        }
        catch (InvalidOperationException)
        {
            _loggerManager.LogWarn("At least one of the specializations from the given list does not exist");
            throw new NotFoundException();
        }

        await _procedureRepository.SaveChangesAsync();
    }

    public async Task DeleteProcedureAsync(int procedureId)
    {
        var procedureToRemove = await _procedureRepository.GetProcedureByIdAsync(procedureId);
        if (procedureToRemove is null)
        {
            _loggerManager.LogWarn($"Procedure with id {procedureId} does not exist");
            throw new NotFoundException($"Procedure with Id {procedureId} does not exist");
        }
        
        _loggerManager.LogInfo($"Deleting procedure with Id {procedureId}...");
        await _procedureRepository.DeleteProcedureAsync(procedureToRemove);
        await _procedureRepository.SaveChangesAsync();
    }


    public async Task<Procedure> GetByIdAsync(int procedureId)
    {
        var procedure = await _procedureRepository.GetProcedureByIdAsync(procedureId);
        
        if (procedure is null)
        {
            _loggerManager.LogWarn($"Procedure with id {procedureId} does not exist");
            throw new NotFoundException($"Procedure with Id {procedureId} does not exist");
        }

        _loggerManager.LogInfo($"Found procedure with id {procedureId}");
        return procedure;
    }

    public async Task<IEnumerable<Procedure>> GetAllProceduresAsync()
    {
        var procedures = await _procedureRepository.GetAllProceduresAsync(); 
        if (procedures is null)
        {
            _loggerManager.LogWarn("Procedures DBSet is null");
            throw new NotFoundException("Procedures DBSet is null");
        }
        
        _loggerManager.LogInfo("Found all procedures");
        return procedures;
    }
}