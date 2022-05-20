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
        Task<Animal> AddNewPetAsync(Animal animal);
        Task<Animal> DeletePetAsync(int id);
        Task<Animal> UpdatePetAsync(Animal animal);
        Task<IEnumerable<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id);
    }
}
