using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAnimalService
    {
        Task CreateAsync(Animal animal);
        Task DeleteAsync(int id);
        Task UpdateAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAsync();
        Task<Animal> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
    }
}
