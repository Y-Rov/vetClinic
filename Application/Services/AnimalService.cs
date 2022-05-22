using Core.Entities;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AnimalService : IAnimalService
    {
        public async Task<Animal> AddNewPetAsync(Animal animal)
        {      
            throw new NotImplementedException();
        }      
               
        public async Task<Animal> DeletePetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Animal> UpdatePetAsync(Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}
