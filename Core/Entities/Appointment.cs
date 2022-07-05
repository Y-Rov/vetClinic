namespace Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool MeetHasOccureding { get; set; }

        public string Disease { get; set; }
        
        public int AnimalId { get; set; }

        public Animal? Animal { get; set; }

        public ICollection<AppointmentUser> AppointmentUsers { get; set; } = new List<AppointmentUser>();

        public ICollection<AppointmentProcedure> AppointmentProcedures { get; set; } = new List<AppointmentProcedure>();
    }
}
