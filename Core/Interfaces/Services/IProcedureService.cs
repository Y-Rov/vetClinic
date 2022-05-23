using Core.Entities;

namespace Core.Interfaces.Services;

public interface IProcedureService
{
    public Task<Procedure> CreateNewProcedureAsync(Procedure procedure);
    public Task UpdateProcedureAsync(int oldProcedureId, Procedure newProcedure);
    public Task DeleteProcedureAsync(int procedureId);

    public Task<Procedure> GetByIdAsync(int procedureId);
    
    public Task<IEnumerable<Procedure>> GetAllProceduresAsync();
}