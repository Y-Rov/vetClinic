using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAnimalService
    {
        Task<Animal> AddNewAnimalAsync(Animal animal);
        Task DeleteAnimalAsync(int id);
        Task UpdateAnimalAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
    }
}
