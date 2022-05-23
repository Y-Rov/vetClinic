using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IProcedureRepository
{
    Task<IEnumerable<Procedure>> GetAllProceduresAsync();
    Task<Procedure?> GetProcedureByIdAsync(int procedureId);
    Task<Procedure> AddProcedureAsync(Procedure procedure);
    Task UpdateProcedureAsync(int procedureId, Procedure newProcedure);
    Task DeleteProcedureAsync(int procedureId);
    Task SaveChangesAsync();
}