using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAnimalRepository
    {
        Task<Animal> AddNewAnimalAsync(Animal animal);
        Task DeleteAnimalAsync(Animal animal);
        Task UpdateAnimalAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<Animal?> GetAnimalByIdAsync(int animalId);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
        Task SaveChangesAsync();
    }
}
