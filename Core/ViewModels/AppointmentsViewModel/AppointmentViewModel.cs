using Core.ViewModels.AppointmentsViewModel;

namespace Core.ViewModels
{
    public class AppointmentViewModel : AppointmentBaseViewModel

    {
        public DateTime Date { get; set; }

        public bool MeetHasOccureding { get; set; }

        public string Disease { get; set; }
    }
}
