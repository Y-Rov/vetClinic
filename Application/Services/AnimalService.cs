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

        public AnimalService(IAnimalRepository animalRepository, ILoggerManager loggerManager)
        {
            _animalRepository = animalRepository;
            _loggerManager = loggerManager;
        }

        public async Task AddNewAnimalAsync(Animal animal)
        {
            await _animalRepository.AddNewAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal created");
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            var animalToDelete = await _animalRepository.GetAnimalByIdAsync(animalId);
            if (animalToDelete == null)
            {
                _loggerManager.LogWarn($"Animal with {animalId} does not exist");
                throw new NotFoundException($"Animal with {animalId} does not exist");
            }
            await _animalRepository.DeleteAnimalAsync(animalToDelete);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal deleted");
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            var animalToUpdate = await _animalRepository.GetAnimalByIdAsync(animal.Id);
            if (animalToUpdate == null)
            {
                _loggerManager.LogWarn($"Animal with {animal.Id} does not exist");
                throw new NotFoundException($"Animal with {animal.Id} does not exist");
            }
            await _animalRepository.UpdateAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal info is up-to-date");
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var animals = await _animalRepository.GetAllAnimalsAsync();
            if (animals is null)
            {
                _loggerManager.LogWarn("Animal list is null");
                throw new NotFoundException();
            }
            _loggerManager.LogInfo($"Getting all-({animals.Count()}) avaliable animals");
            return animals;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var appointments = await _animalRepository.GetAllAppointmentsWithAnimalIdAsync(id);
            if (appointments is null)
            {
                _loggerManager.LogWarn("Appointment list is null");
                throw new NotFoundException();
            }
            _loggerManager.LogInfo($"Getting all-({appointments.Count()}) avaliable appointments for the animal");
            return appointments;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (animal == null)
            {
                _loggerManager.LogWarn($"Animal with Id - {id} does not exist");
                throw new NotFoundException();
            }
            _loggerManager.LogInfo($"Aniaml with Id - {id} was found");
            return animal;
        }
    }
}
