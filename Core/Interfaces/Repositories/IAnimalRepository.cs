using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAnimalRepository
    {
        Task<Animal> CreateAsync(Animal animal);
        Task DeleteAsync(Animal animal);
        Task UpdateAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAsync();
        Task<Animal?> GetAsync(int animalId);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
        Task SaveChangesAsync();
    }
}
