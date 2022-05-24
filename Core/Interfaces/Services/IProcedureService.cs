using Core.Entities;

namespace Core.Interfaces.Services;

public interface IProcedureService
{
    public Task<Procedure> CreateNewProcedureAsync(Procedure procedure);
    public Task UpdateProcedureAsync(Procedure newProcedure);
    Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds);

    public Task DeleteProcedureAsync(int procedureId);

    public Task<Procedure> GetByIdAsync(int procedureId);
    
    public Task<IEnumerable<Procedure>> GetAllProceduresAsync();
}