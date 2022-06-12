using Core.Entities;

namespace Core.Interfaces.Services;

public interface IProcedureService
{
    Task CreateNewProcedureAsync(Procedure procedure);
    Task UpdateProcedureAsync(Procedure newProcedure);
    Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds);

    Task DeleteProcedureAsync(int procedureId);

    Task<Procedure> GetByIdAsync(int procedureId);
    
    Task<IEnumerable<Procedure>> GetAllProceduresAsync();
}