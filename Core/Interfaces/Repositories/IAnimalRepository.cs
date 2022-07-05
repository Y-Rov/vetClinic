using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Repositories
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        Task<PagedList<Appointment>> GetAllAppointmentsWithAnimalIdAsync(AnimalParameters animalParameters);
    }
}
