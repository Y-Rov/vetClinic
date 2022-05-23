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
        return await _procedureRepository.AddProcedureAsync(procedure);
    }
    public async Task UpdateProcedureAsync(int oldProcedureId, Procedure newProcedure)
    {
        await _procedureRepository.UpdateProcedureAsync(oldProcedureId, newProcedure);
    }

    public async Task DeleteProcedureAsync(int procedureId)
    {
        await _procedureRepository.DeleteProcedureAsync(procedureId);
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