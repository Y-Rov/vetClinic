using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly ClinicContext _clinicContext;

        public AnimalRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<Animal> AddNewAnimalAsync(Animal animal)
        {
            var newAnimal =  await _clinicContext.Animals.AddAsync(animal);
            return newAnimal.Entity;
        }

        public async Task DeleteAnimalAsync(Animal animal)
        {
            _clinicContext.Animals.Remove(animal);
        }

        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            var result = await _clinicContext.Animals
                .Include(pet => pet.Owner)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var result = await _clinicContext.Appointments
                .Where(appointment => appointment.AnimalId == id)
                .ToListAsync();
            return result;
        }

        public async Task<Animal?> GetAnimalByIdAsync(int animalId)
        {
            var animal = await _clinicContext.Animals
                .AsNoTracking()
                .SingleOrDefaultAsync(animal => animal.Id == animalId);
            return animal;
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            _clinicContext.Animals.Update(animal);
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
