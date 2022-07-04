using Core.Entities;

namespace Core.Interfaces.Services;

public interface IProcedureService
{
    Task CreateNewProcedureAsync(Procedure procedure, IEnumerable<int> specializationIds);
    Task UpdateProcedureAsync(Procedure newProcedure, IEnumerable<int> specializationIds);
    Task DeleteProcedureAsync(int procedureId);
    Task<Procedure> GetByIdAsync(int procedureId);
    Task<IEnumerable<Procedure>> GetAllProceduresAsync();
}