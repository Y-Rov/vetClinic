using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures;

public class ProcedureServiceFixture
{
    public ProcedureServiceFixture()
    {
        var fixture =
            new Fixture().Customize(new AutoMoqCustomization());

        MockProcedureRepository = fixture.Freeze<Mock<IProcedureRepository>>();
        MockSpecializationService = fixture.Freeze<Mock<ISpecializationService>>();
        MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
        MockProcedureService = new ProcedureService(
            MockProcedureRepository.Object,
            MockSpecializationService.Object,
            MockLoggerManager.Object);
    }
    
    public IProcedureService MockProcedureService { get; }
    public Mock<IProcedureRepository> MockProcedureRepository { get; }
    public Mock<ISpecializationService> MockSpecializationService { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
}