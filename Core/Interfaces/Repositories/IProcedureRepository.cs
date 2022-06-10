using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IProcedureRepository : IRepository<Procedure>
{
    Task UpdateProcedureSpecializationsAsync(int procedureId, IEnumerable<int> specializationIds);
}