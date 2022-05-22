using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IProcedureRepository
{
    Task<IEnumerable<Procedure>> GetAllProceduresAsync();
    Task<Procedure?> GetProcedureByIdAsync(int procedureId);
    void AddProcedureAsync(Procedure procedure);
    void UpdateProcedureAsync(Procedure newProcedure);
    void DeleteProcedureAsync(int procedureId);
    Task SaveChangesAsync();
}