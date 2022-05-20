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
        public void AddNewPet(Animal animal)
        {
            throw new NotImplementedException();
        }

        public void DeletePet(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePet(Animal animal)
        {
            throw new NotImplementedException();
        }

        public Animal GetAnimalById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Animal> GetAllAnimals()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Appointment> GetAllAppointmentsWithAnimalId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
