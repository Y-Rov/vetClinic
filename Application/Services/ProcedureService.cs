using Core.Entities;
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

    public async Task UpdateProcedureAsync(int oldProcedureId, Procedure newProcedure)   
    {
        await _procedureRepository.UpdateProcedureAsync(oldProcedureId, newProcedure);
        await _procedureRepository.SaveChangesAsync();
    }

    public async Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds)
    {
        await _procedureRepository.UpdateProcedureSpecializationsAsync(procedureId, specializationIds);
        await _procedureRepository.SaveChangesAsync();
    }

    public async Task DeleteProcedureAsync(int procedureId)
    {
        var procedureToRemove = await _procedureRepository.GetProcedureByIdAsync(procedureId);
        if (procedureToRemove is null) throw new NullReferenceException();
        await _procedureRepository.DeleteProcedureAsync(procedureToRemove);
        await _procedureRepository.SaveChangesAsync();
    }


    public async Task<Procedure> GetByIdAsync(int procedureId)
    { 
       var result= await _procedureRepository.GetProcedureByIdAsync(procedureId);
       if (result is null) throw new NullReferenceException();
       return result;
    }

    public async Task<IEnumerable<Procedure>> GetAllProceduresAsync()
    {
        var result = await _procedureRepository.GetAllProceduresAsync(); 
        if (result is null) throw new NullReferenceException();
        return result;
    }
}