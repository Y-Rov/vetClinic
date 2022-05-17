using Core.Entities;

namespace Core.Interfaces
{
    public interface IAnimalService
    {
        public void AddNewPet();
        public void DeletePet();
        public void UpdatePet();
        public IEnumerable<Animal> GetAllAnimals();
        public Animal GetAnimalById(int id);
        //public IEnumerable<Appoinment> GetAllAppointmentsWithAnimalId(int id); - Appointment entity has not created yet
        
    }
}
