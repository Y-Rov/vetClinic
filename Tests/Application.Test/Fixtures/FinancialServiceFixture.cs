using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures
{
    public class FinancialServiceFixture
    {
        public FinancialServiceFixture()
        {
            var fixture =
                new Fixture().Customize(new AutoMoqCustomization());

            MockSalaryRepository = fixture.Freeze<Mock<ISalaryRepository>>();
            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
            MockAppointmentRepository = fixture.Freeze<Mock<IAppointmentRepository>>();
            MockProcedureRepository = fixture.Freeze<Mock<IProcedureRepository>>();

            MockFinancialService = new FinancialService(
                MockSalaryRepository.Object,
                MockUserRepository.Object,
                MockLoggerManager.Object,
                MockAppointmentRepository.Object,
                MockProcedureRepository.Object);
        }

        public IFinancialService MockFinancialService { get; }
        public Mock<ISalaryRepository> MockSalaryRepository { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Mock<IAppointmentRepository> MockAppointmentRepository { get; }
        public Mock<IProcedureRepository> MockProcedureRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
    }
}
