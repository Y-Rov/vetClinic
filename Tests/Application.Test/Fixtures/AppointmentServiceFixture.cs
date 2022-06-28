using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
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

            MockAppointmentEntityService = fixture.Freeze<IAppointmentService>();
            MockProcedureEntityService = fixture.Freeze<Mock<IProcedureService>>();
            MockUserEntityService = fixture.Freeze<Mock<IUserService>>();
            MockAppointmentRepository = fixture.Freeze<Mock<IAppointmentRepository>>();
            MockLogger = fixture.Freeze<Mock<ILoggerManager>>();

            MockAppointmentEntityService = new AppointmentService(
                MockAppointmentRepository.Object,
                MockProcedureEntityService.Object,
                MockUserEntityService.Object,
                MockLogger.Object);


        }

        public IAppointmentService MockAppointmentEntityService { get; }
        public Mock<IProcedureService> MockProcedureEntityService { get; }
        public Mock<IUserService> MockUserEntityService { get; }
        public Mock<IAppointmentRepository> MockAppointmentRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }

    }
}
