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

        public async Task<Animal> CreateAsync(Animal animal)
        {
            var newAnimal =  await _clinicContext.Animals.AddAsync(animal);
            return newAnimal.Entity;
        }

        public async Task DeleteAsync(Animal animal)
        {
            _clinicContext.Animals.Remove(animal);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Animal>> GetAsync()
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

        public async Task<Animal?> GetAsync(int animalId)
        {
            var animal = await _clinicContext.Animals
                .Include(pet => pet.Owner)
                .AsNoTracking()
                .FirstOrDefaultAsync(animal => animal.Id == animalId);
            return animal;
        }

        public async Task UpdateAsync(Animal animal)
        {
            _clinicContext.Animals.Update(animal);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
