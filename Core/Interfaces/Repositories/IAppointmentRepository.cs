using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAsync();
        Task<Appointment> GetAsync(int appointmentId);
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointmentId);
        Task SaveChangesAsync();
    }
}
