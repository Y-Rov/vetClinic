using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISpecializationRepository : IRepository<Specialization>
    {
        public Task UpdateProceduresAsync(int specializationId, IEnumerable<int> procedureIds);
    }
}
