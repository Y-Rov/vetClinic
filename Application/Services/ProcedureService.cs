using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

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
        var createdProcedure = await _procedureRepository.AddProcedureAsync(procedure);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Created new procedure with Id: {createdProcedure.Id}");
        return createdProcedure;
    }

    public async Task UpdateProcedureAsync(Procedure newProcedure)
    {
        var oldProcedure = await GetByIdAsync(newProcedure.Id);

        await _procedureRepository.UpdateProcedureAsync(newProcedure);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated procedure with id {newProcedure.Id}");
    }

    public async Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds)
    {
        var procedureToUpdate = await GetByIdAsync(procedureId);
        try
        {
            await _procedureRepository.UpdateProcedureSpecializationsAsync(procedureToUpdate, specializationIds);
        }
        catch (InvalidOperationException)
        {
            _loggerManager.LogWarn("At least one of the specializations from the given list does not exist");
            throw new NotFoundException("At least one of the specializations from the given list does not exist");
        }

        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Updated specializations list of the procedure with Id {procedureId}");
    }

    public async Task DeleteProcedureAsync(int procedureId)
    {
        var procedureToRemove = await GetByIdAsync(procedureId);
        
        await _procedureRepository.DeleteProcedureAsync(procedureToRemove);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Deleted procedure with Id {procedureId}");
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
        _loggerManager.LogInfo("Found all procedures");
        return procedures;
    }
}