using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IProcedureRepository
{
    Task<IEnumerable<Procedure>> GetAllProceduresAsync();
    Task<Procedure?> GetProcedureByIdAsync(int procedureId);
    Task<Procedure> AddProcedureAsync(Procedure procedure);
    Task UpdateProcedureAsync(int procedureId, Procedure newProcedure);
    Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds);
    Task DeleteProcedureAsync(Procedure procedure);
    Task SaveChangesAsync();
}