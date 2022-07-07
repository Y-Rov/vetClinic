using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures
{
    public class AppointmentServiceFixture
    {

        public AppointmentServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAppointment = GenerateAppointment();
            MockProcedure = GenerateProcedure();
            MockUser = GenerateUser();
            MockListAppointments = GenerateListAppointments();

            MockAppointmentEntityService = fixture.Freeze<IAppointmentService>();
            MockProcedureEntityService = fixture.Freeze<Mock<IProcedureService>>();
            MockUserEntityService = fixture.Freeze<Mock<IUserService>>();
            MockAppointmentRepository = fixture.Freeze<Mock<IAppointmentRepository>>();
            MockLogger = fixture.Freeze<Mock<ILoggerManager>>();
            MockAppointmentUserRepository = fixture.Freeze<Mock<IAppointmentUserRepository>>();
            MockAppointmentProcedureRepository = fixture.Freeze<Mock<IAppointmentProcedureRepository>>();

            MockAppointmentEntityService = new AppointmentService(
                MockAppointmentRepository.Object,
                MockProcedureEntityService.Object,
                MockUserEntityService.Object,
                MockAppointmentUserRepository.Object,
                MockAppointmentProcedureRepository.Object,
                MockLogger.Object);


        }

        public IAppointmentService MockAppointmentEntityService { get; }
        public Mock<IProcedureService> MockProcedureEntityService { get; }
        public Mock<IUserService> MockUserEntityService { get; }
        public Mock<IAppointmentRepository> MockAppointmentRepository { get; }
        public Mock<IAppointmentUserRepository> MockAppointmentUserRepository { get; }
        public Mock<IAppointmentProcedureRepository> MockAppointmentProcedureRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }

        public Appointment MockAppointment { get; set; }
        public Procedure MockProcedure { get; set; }
        public User MockUser { get; set; }
        public IList<Appointment> MockListAppointments { get; set; }

        private Appointment GenerateAppointment()
        {
            var appointment = new Appointment
            {
                Id = 1,
                Disease = "Pain",
                Date = DateTime.Now,
                MeetHasOccureding = true,
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

            return appointment;
        }

        private Procedure GenerateProcedure()
        {
            var procedure = new Procedure
            {
                Id = 1,
                Name = "haircut",
                Description = "haircut description",
                DurationInMinutes = 35
            };

            return procedure;
        }


        private  User GenerateUser()
        {
            var user = new User
            {
                FirstName = "Tom",
                LastName = "Smith",
                BirthDate = DateTime.Now,
                IsActive = true,
                ProfilePicture = { }
            };

            return user;    
        }

        private readonly IEnumerable<int> _procedureIds = new List<int>()
    {
        1, 2, 5, 6
    };

        private readonly IEnumerable<int> _userIds = new List<int>()
    {
        1, 2, 5, 6
    };

        private IList<Appointment> GenerateListAppointments()
        {
            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    Disease = "Pain",
                    Date = DateTime.Now,
                    MeetHasOccureding = true
                },

                new Appointment
                {
                    Id = 3,
                    Disease = "Pain",
                    Date = DateTime.Now,
                    MeetHasOccureding = true
                }
            };

            return appointments;
        }
    }
}
