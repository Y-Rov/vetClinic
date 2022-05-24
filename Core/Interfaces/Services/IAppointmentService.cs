using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAsync();
        Task<Appointment> GetAsync(int appointmentId);
        Task CreateAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int appointmentId);
    }
}
