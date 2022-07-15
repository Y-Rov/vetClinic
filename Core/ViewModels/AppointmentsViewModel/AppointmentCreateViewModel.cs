namespace Core.ViewModels.AppointmentsViewModel
{
    public class AppointmentCreateViewModel : AppointmentBaseViewModel
    {
        public IEnumerable<int> ProcedureIds { get; set; } = new List<int>();

        public IEnumerable<int> UserIds { get; set; } = new List<int>();

        public int AnimalId { get; set; }
    }
}
