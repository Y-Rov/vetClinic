using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AnimalRepository : Repository<Animal>, IAnimalRepository
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AnimalRepository(ClinicContext clinicContext, IAppointmentRepository appointmentRepository) : base(clinicContext)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsWithAnimalIdAsync(int id)
        {
            var result = await _appointmentRepository.GetAsync(filter: app => app.AnimalId == id);
            return result;
        }
    }
}
