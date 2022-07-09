using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        //Task UpdateAppointmentProceduresAsync(int appointmentId, IEnumerable<int> procedureIds);
        //Task UpdateAppointmentUsersAsync(int appointmentId, IEnumerable<int> userIds);
        //IEnumerable<User> GetDoctorsAsync();
    }
}
