using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISpecializationRepository : IRepository<Specialization>
    {
        Task UpdateProceduresAsync(int specializationId, IEnumerable<int> procedureIds);
        Task UpdateUsersAsync(int specializationId, IEnumerable<int> userIds);
    }
}
