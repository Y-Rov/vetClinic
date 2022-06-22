using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures;

public class ProcedureControllerFixture
{
    public ProcedureControllerFixture()
    {
        var fixture = 
            new Fixture().Customize(new AutoMoqCustomization());

        MockProcedureService = fixture.Freeze<Mock<IProcedureService>>();
        MockCreateSpecializationMapper = fixture.Freeze<Mock<IViewModelMapper<ProcedureCreateViewModel, Procedure>>>();
        MockUpdateSpecializationMapper = fixture.Freeze<Mock<IViewModelMapper<ProcedureUpdateViewModel, Procedure>>>();
        MockProcedureReadViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<Procedure, ProcedureReadViewModel>>>();
        MockProcedureReadViewModelListMapper = fixture
            .Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>>>();

        MockProcedureController = new ProcedureController(
            MockProcedureService.Object,
            MockCreateSpecializationMapper.Object,
            MockUpdateSpecializationMapper.Object,
            MockProcedureReadViewModelMapper.Object,
            MockProcedureReadViewModelListMapper.Object);
    }
    
    public ProcedureController MockProcedureController { get; }
    public Mock<IProcedureService> MockProcedureService { get; }
    public Mock<IViewModelMapper<ProcedureCreateViewModel, Procedure>> MockCreateSpecializationMapper { get; }
    public Mock<IViewModelMapper<ProcedureUpdateViewModel, Procedure>> MockUpdateSpecializationMapper { get; }
    public Mock<IViewModelMapper<Procedure, ProcedureReadViewModel>> MockProcedureReadViewModelMapper { get; }
    public Mock<IEnumerableViewModelMapper<IEnumerable<Procedure>, IEnumerable<ProcedureReadViewModel>>> MockProcedureReadViewModelListMapper { get; }
}