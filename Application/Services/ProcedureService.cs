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

    public async Task CreateNewProcedureAsync(Procedure procedure)
    {
        await _procedureRepository.InsertAsync(procedure);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Created new procedure with name: {procedure.Name}");
    }

    public async Task UpdateProcedureAsync(Procedure newProcedure)
    {
        var oldProcedure = await GetByIdAsync(newProcedure.Id);

        _procedureRepository.Update(newProcedure);
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
        
        _procedureRepository.Delete(procedureToRemove);
        await _procedureRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Deleted procedure with Id {procedureId}");
    }


    public async Task<Procedure> GetByIdAsync(int procedureId)
    {
        var procedure = await _procedureRepository.GetFirstOrDefaultAsync(
            filter: pr => pr.Id == procedureId,
            includeProperties: "ProcedureSpecializations.Specialization",
            asNoTracking: true);
        
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
        var procedures = await _procedureRepository.GetAsync(
            includeProperties:"ProcedureSpecializations.Specialization",
            asNoTracking: true);
        _loggerManager.LogInfo("Found all procedures");
        return procedures;
    }
}