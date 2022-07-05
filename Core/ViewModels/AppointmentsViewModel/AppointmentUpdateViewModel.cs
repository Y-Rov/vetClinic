namespace Core.ViewModels.AppointmentsViewModel
{
    public class AppointmentUpdateViewModel : AppointmentBaseViewModel
    {
        public int Id { get; set; }

        public IEnumerable<int> ProcedureIds { get; set; } = new List<int>();

        public IEnumerable<int> UserIds { get; set; } = new List<int>();

        public int AnimalId { get; set; }
    }
}
