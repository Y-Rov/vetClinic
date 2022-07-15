using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Application.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly ILoggerManager _loggerManager;
        private readonly IAnimalPhotoService _animalPhotoService;

        public AnimalService(
            IAnimalRepository animalRepository,
            ILoggerManager loggerManager,
            IAnimalPhotoService animalPhotoService
            )
        {
            _animalRepository = animalRepository;
            _loggerManager = loggerManager;
            _animalPhotoService = animalPhotoService;
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

            await _animalPhotoService.DeleteAsync(animalToDelete.PhotoUrl!);

            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal deleted");
        }

        public async Task UpdateAsync(Animal animal)
        {
            _animalRepository.Update(animal);
            await _animalRepository.SaveChangesAsync();
            _loggerManager.LogInfo("Animal info is up-to-date");
        }

        public async Task<IEnumerable<Animal>> GetAsync(int ownerId)
        {
            var animals = await _animalRepository.GetAsync(filter: animal => animal.OwnerId == ownerId);
            _loggerManager.LogInfo($"A list of animals with lenght = {animals.Count()} is been returned");
            return animals;
        }

        public async Task<PagedList<Appointment>> GetAllAppointmentsWithAnimalIdAsync(AnimalParameters animalParameters)
        {
            var appointments = await _animalRepository.GetAllAppointmentsWithAnimalIdAsync(animalParameters);
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
