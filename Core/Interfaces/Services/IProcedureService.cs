using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services;

public interface IProcedureService
{
    Task CreateNewProcedureAsync(Procedure procedure, IEnumerable<int> specializationIds);
    Task UpdateProcedureAsync(Procedure newProcedure, IEnumerable<int> specializationIds);
    Task DeleteProcedureAsync(int procedureId);
    Task<Procedure> GetByIdAsync(int procedureId);
    Task<PagedList<Procedure>> GetAllProceduresAsync(ProcedureParameters parameters);
}