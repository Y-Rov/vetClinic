using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<PagedList<Appointment>> GetAsync(AppointmentParameters parameters);
        Task<Appointment> GetAsync(int appointmentId);
        Task CreateAsync(Appointment appointment, IEnumerable<int> procedureIds, IEnumerable<int> userIds, int animalId);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int appointmentId);
        Task UpdateAppointmentProceduresAsync(int appointmentId, IEnumerable<int> procedureIds);
        Task UpdateAppointmentUsersAsync(int appointmentId, IEnumerable<int> userIds);
    }
}
