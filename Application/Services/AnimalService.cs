using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public async Task AddNewAnimalAsync(Animal animal)
        {
            await _animalRepository.AddNewAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            var animalToDelete = await _animalRepository.GetAnimalByIdAsync(animalId);
            if (animalToDelete == null)
            {
                throw new ArgumentNullException(nameof(animalToDelete));
            }
            await _animalRepository.DeleteAnimalAsync(animalToDelete);
            await _animalRepository.SaveChangesAsync();
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            if (animal == null)
            {
                throw new ArgumentNullException(nameof(animal));
            }
            await _animalRepository.UpdateAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var animals = await _animalRepository.GetAllAnimalsAsync();
            if (animals is null)
            {
                throw new ArgumentNullException(nameof(animals));
            }
            return animals;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var appointments = await _animalRepository.GetAllAppointmentsWithAnimalIdAsync(id);
            if (appointments is null)
            {
                throw new ArgumentNullException(nameof(appointments));
            }
            return appointments;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (animal is null)
            {
                throw new ArgumentNullException();
            }
            return animal;
        }
    }
}
