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
        Task<Animal> AddNewPet(Animal animal);
        Task<Animal> DeletePet(int id);
        Task<Animal> UpdatePet(Animal animal);
        Task<IEnumerable<Animal>> GetAllAnimals();
        Task<Animal> GetAnimalById(int id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalId(int id);
    }
}
