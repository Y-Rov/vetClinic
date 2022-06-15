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

        public async Task CreateAsync(Animal animal)
        {
            await _animalRepository.InsertAsync(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal created");
        }

        public async Task DeleteAsync(int animalId)
        {
            var animalToDelete = await GetByIdAsync(animalId);
            _animalRepository.Delete(animalToDelete);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal deleted");
        }

        public async Task UpdateAsync(Animal animal)
        {
            _animalRepository.Update(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal info is up-to-date");
        }

        public async Task<IEnumerable<Animal>> GetAsync()
        {
            var animals = await _animalRepository.GetAsync();
            _loggerManager.LogInfo($"A list of animals with lenght = {animals.Count()} is been returned");
            return animals;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var appointments = await _animalRepository.GetAllAppointmentsWithAnimalIdAsync(id);
            _loggerManager.LogInfo($"A list of animal-specific appointments with lenght = {appointments.Count()} is been returned");
            return appointments;
        }

        public async Task<Animal> GetByIdAsync(int animalId)
        {
            var animal = await _animalRepository.GetById(animalId);
            if (animal == null)
            {
                _loggerManager.LogWarn($"Animal with Id = {animalId} does not exist");
                throw new NotFoundException();
            }
            _loggerManager.LogInfo($"Aniaml with Id = {animalId} was found");
            return animal;
        }
    }
}
