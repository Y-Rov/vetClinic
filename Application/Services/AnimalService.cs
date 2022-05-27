using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly ILoggerManager _loggerManager;

        public AnimalService(
            IAnimalRepository animalRepository,
            ILoggerManager loggerManager)
        {
            _animalRepository = animalRepository;
            _loggerManager = loggerManager;
        }

        public async Task<Animal> AddNewAnimalAsync(Animal animal)
        {
            var newAnimal =  await _animalRepository.AddNewAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal created");
            return newAnimal;
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            var animalToDelete = await GetAnimalByIdAsync(animalId);
            await _animalRepository.DeleteAnimalAsync(animalToDelete);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal deleted");
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            var animalToUpdate = await GetAnimalByIdAsync(animal.Id);
            await _animalRepository.UpdateAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal info is up-to-date");
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var animals = await _animalRepository.GetAllAnimalsAsync();
            _loggerManager.LogInfo($"A list of animals with lenght = {animals.Count()} is been returned");
            return animals;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var appointments = await _animalRepository.GetAllAppointmentsWithAnimalIdAsync(id);
            _loggerManager.LogInfo($"A list of animal-specific appointments with lenght = {appointments.Count()} is been returned");
            return appointments;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                _loggerManager.LogWarn($"Animal with Id = {id} does not exist");
                throw new NotFoundException();
            }
            _loggerManager.LogInfo($"Aniaml with Id = {id} was found");
            return animal;
        }
    }
}
