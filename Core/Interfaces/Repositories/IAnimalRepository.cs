using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
    }
}
