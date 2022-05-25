using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IUserRepository _userRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public async Task AddNewAnimalAsync(Animal animal)
        {
            //-----------
            //var owner = _userRepository.GetUserByIdAsync(animal.OwnerId);
            //if(owner == null)
            //{
            //    throw new NotFoundException();
            //}
            //----------
            await _animalRepository.AddNewAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            var animalToDelete = await _animalRepository.GetAnimalByIdAsync(animalId);
            if (animalToDelete == null)
            {
                throw new NotFoundException($"Animal with {animalId} does not exist");
            }
            await _animalRepository.DeleteAnimalAsync(animalToDelete);
            await _animalRepository.SaveChangesAsync();
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            var animalToUpdate = await _animalRepository.GetAnimalByIdAsync(animal.Id);
            if (animalToUpdate == null)
            {
                throw new NotFoundException($"Animal does not exist");
            }
            await _animalRepository.UpdateAnimalAsync(animal);
            await _animalRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var animals = await _animalRepository.GetAllAnimalsAsync();
            if (animals is null)
            {
                throw new NotFoundException();
            }
            return animals;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var appointments = await _animalRepository.GetAllAppointmentsWithAnimalIdAsync(id);
            if (appointments is null)
            {
                throw new NotFoundException();
            }
            return appointments;
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            var animal = await _animalRepository.GetAnimalByIdAsync(id);
            if (animal is null)
            {
                throw new NotFoundException();
            }
            return animal;
        }
    }
}
