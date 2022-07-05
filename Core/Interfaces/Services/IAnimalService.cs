using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface IAnimalService
    {
        Task CreateAsync(Animal animal);
        Task DeleteAsync(int id);
        Task UpdateAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAsync();
        Task<Animal> GetByIdAsync(int id);
        Task<PagedList<Appointment>> GetAllAppointmentsWithAnimalIdAsync(AnimalParameters animalParameters);
    }
}
