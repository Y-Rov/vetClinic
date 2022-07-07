using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface IAnimalService
    {
        Task CreateAsync(Animal animal);
        Task DeleteAsync(int animalId);
        Task UpdateAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAsync(int ownerId);
        Task<Animal> GetByIdAsync(int animalId);
        Task<PagedList<Appointment>> GetAllAppointmentsWithAnimalIdAsync(AnimalParameters animalParameters);
    }
}
