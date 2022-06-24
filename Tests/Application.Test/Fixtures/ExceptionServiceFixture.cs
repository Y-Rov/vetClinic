using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;

namespace Application.Test.Fixtures
{
    public class ExceptionServiceFixture
    {
        public ExceptionServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockExceptionEntityService = fixture.Freeze<ExceptionEntityService>();
            MockExceptionRepository = fixture.Freeze<Mock<IExceptionEntityRepository>>();
            MockLogger = fixture.Freeze<Mock<ILoggerManager>>();

            MockExceptionEntityService = new ExceptionEntityService(
                MockExceptionRepository.Object,
                MockLogger.Object);
        }

        public ExceptionEntityService MockExceptionEntityService { get; }
        public Mock<IExceptionEntityRepository> MockExceptionRepository { get; }
        public Mock<ILoggerManager> MockLogger { get; }
    }
}
