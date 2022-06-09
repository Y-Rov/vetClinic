using Core.Entities;

namespace Core.Interfaces.Repositories;

public interface IProcedureRepository : IRepository<Procedure>
{
    Task UpdateProcedureSpecializationsAsync(Procedure procedure, IEnumerable<int> specializationIds);
}