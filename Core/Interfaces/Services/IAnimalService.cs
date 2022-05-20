using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IAnimalService
    {
        public void AddNewPet(Animal animal);
        public void DeletePet(int id);
        public void UpdatePet(Animal animal);
        public IEnumerable<Animal> GetAllAnimals();
        public Animal GetAnimalById(int id);
        public IEnumerable<Appointment> GetAllAppointmentsWithAnimalId(int id);
    }
}
