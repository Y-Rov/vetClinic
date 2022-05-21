using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAnimalService
    {
        Task<Animal> AddNewPetAsync(Animal animal);
        Task<Animal> DeletePetAsync(int id);
        Task<Animal> UpdatePetAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
    }
}
