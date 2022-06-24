using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;

namespace Application.Test.Fixtures
{
    public class AppointmentServiceFixture
    {

        //public AppointmentServiceFixture()
        //{
        //    var fixture = new Fixture().Customize(new AutoMoqCustomization());
            
        //    MockAppointmentEntityService = fixture.Freeze<AppointmentService>();
        //    MockProcedureEntityService = fixture.Freeze<ProcedureService>();
        //    MockUserEntityService = fixture.Freeze<UserService>();
        //    MockAppointmentRepository = fixture.Freeze<Mock<IAppointmentRepository>>();
        //    MockLogger = fixture.Freeze<Mock<ILoggerManager>>();

        //    MockAppointmentEntityService = new AppointmentService(
        //        MockExceptionRepository.Object,
        //        MockLogger.Object);


        //}

    }
}
