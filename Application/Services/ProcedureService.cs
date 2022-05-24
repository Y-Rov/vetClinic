using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services;

public class ProcedureService : IProcedureService
{
    private readonly IProcedureRepository _procedureRepository;
    public ProcedureService(IProcedureRepository procedureRepository)
    {
        _procedureRepository = procedureRepository;
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
            throw new NotFoundException($"Procedure with Id {newProcedure.Id} does not exist");
        }
        
        oldProcedure.Cost = newProcedure.Cost;
        oldProcedure.DurationInMinutes = newProcedure.DurationInMinutes;
        oldProcedure.Name = newProcedure.Name;
        oldProcedure.Description = newProcedure.Description;        
        await _procedureRepository.SaveChangesAsync();
    }

    public async Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds)
    {
        try
        {
            await _procedureRepository.UpdateProcedureSpecializationsAsync(procedureId, specializationIds);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException();
        }

        await _procedureRepository.SaveChangesAsync();
    }

    public async Task DeleteProcedureAsync(int procedureId)
    {
        var procedureToRemove = await _procedureRepository.GetProcedureByIdAsync(procedureId);
        if (procedureToRemove is null)
        {
            throw new NotFoundException($"Procedure with Id {procedureId} does not exist");
        }
        await _procedureRepository.DeleteProcedureAsync(procedureToRemove);
        await _procedureRepository.SaveChangesAsync();
    }


    public async Task<Procedure> GetByIdAsync(int procedureId)
    {
        var procedure = await _procedureRepository.GetProcedureByIdAsync(procedureId);
        
        if (procedure is null)
        {
            throw new NotFoundException($"Procedure with Id {procedureId} does not exist");
        }

        return procedure;
    }

    public async Task<IEnumerable<Procedure>> GetAllProceduresAsync()
    {
        var procedures = await _procedureRepository.GetAllProceduresAsync(); 
        if (procedures is null)
        {
            throw new NotFoundException("Procedures DBSet is null");
        }
        return procedures;
    }
}