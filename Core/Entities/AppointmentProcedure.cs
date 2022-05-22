namespace Core.Entities
{
    public class AppointmentProcedure
    {
        public Appointment Appointment { get; set; }

        public int AppointmentId { get; set; }

        public int ProcedureId { get; set; }

        public Procedure Procedure { get; set; }
    }
}
