using Core.Entities;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class AppointmentControllerTests : IClassFixture<AppointmentControllerFixture>
    {
        public AppointmentControllerTests(AppointmentControllerFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly AppointmentControllerFixture _fixture;

        private readonly Appointment _appointment = new Appointment()
        {
            Date = DateTime.Now,
            MeetHasOccureding = true,
            Disease = "Broke a leg",
            AnimalId = 3,
            AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

            AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
        };
    }
}
