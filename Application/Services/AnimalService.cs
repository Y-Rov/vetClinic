using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AnimalService : IAnimalService
    {
        public async Task<Animal> AddNewPet(Animal animal)
        {      
            throw new NotImplementedException();
        }      
               
        public async Task<Animal> DeletePet(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Animal>> GetAllAnimals()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Animal> UpdatePet(Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}
