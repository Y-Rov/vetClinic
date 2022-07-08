using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.ViewModels;
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
        MockCreateProcedureMapper = fixture.Freeze<Mock<IViewModelMapper<ProcedureCreateViewModel, Procedure>>>();
        MockUpdateProcedureMapper = fixture.Freeze<Mock<IViewModelMapper<ProcedureUpdateViewModel, Procedure>>>();
        MockProcedureReadViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<Procedure, ProcedureReadViewModel>>>();
        MockPagedListMapper = fixture
            .Freeze<Mock<IViewModelMapper<PagedList<Procedure>, PagedReadViewModel<ProcedureReadViewModel>>>>();

        MockProcedureController = new ProcedureController(
            MockProcedureService.Object,
            MockCreateProcedureMapper.Object,
            MockUpdateProcedureMapper.Object,
            MockProcedureReadViewModelMapper.Object,
            MockPagedListMapper.Object);
    }

    public ProcedureController MockProcedureController { get; }
    public Mock<IProcedureService> MockProcedureService { get; }
    public Mock<IViewModelMapper<ProcedureCreateViewModel, Procedure>> MockCreateProcedureMapper { get; }
    public Mock<IViewModelMapper<ProcedureUpdateViewModel, Procedure>> MockUpdateProcedureMapper { get; }
    public Mock<IViewModelMapper<Procedure, ProcedureReadViewModel>> MockProcedureReadViewModelMapper { get; }
    public Mock<IViewModelMapper<PagedList<Procedure>, PagedReadViewModel<ProcedureReadViewModel>>> MockPagedListMapper { get; }
}