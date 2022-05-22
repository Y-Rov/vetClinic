namespace Core.Entities
{
    public class AppointmentUser
    {
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
