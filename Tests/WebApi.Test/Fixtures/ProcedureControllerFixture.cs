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

        Procedure = GetProcedure();
        ProcedureCreateViewModel = GetProcedureCreateViewModel();
        ProcedureUpdateViewModel = GetProcedureUpdateViewModel();
        ExpectedProcedureReadViewModel = GetExpectedProcedureReadViewModel();
        PagedProcedures = GetPagedProcedures();
        PagedReadViewModel = GetPagedReadViewModel();
        
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
    
    public Procedure Procedure { get; }
    public ProcedureCreateViewModel ProcedureCreateViewModel { get; }
    public ProcedureUpdateViewModel ProcedureUpdateViewModel { get; }
    public ProcedureReadViewModel ExpectedProcedureReadViewModel { get; }
    public PagedList<Procedure> PagedProcedures { get; }
    public PagedReadViewModel<ProcedureReadViewModel> PagedReadViewModel { get; }

    private Procedure GetProcedure()
    {
        var procedure = new Procedure()
        {
            Id = 13,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            ProcedureSpecializations = new List<ProcedureSpecialization>()
            {
                new ProcedureSpecialization() {ProcedureId = 13, SpecializationId = 17},
                new ProcedureSpecialization() {ProcedureId = 13, SpecializationId = 18},
            }
        };
        return procedure;
    }

    private ProcedureCreateViewModel GetProcedureCreateViewModel()
    {
        var procedureCreateViewModel = new ProcedureCreateViewModel ()
        {
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                17, 18
            }
        };
        return procedureCreateViewModel;
    }

    private ProcedureUpdateViewModel GetProcedureUpdateViewModel()
    {
        var procedureUpdateViewModel = new ProcedureUpdateViewModel()
        {
            Id = 13,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                17, 18
            }
        };
        return procedureUpdateViewModel;
    }

    private ProcedureReadViewModel GetExpectedProcedureReadViewModel()
    {
        var procedureReadViewModel = new ProcedureReadViewModel()
        {
            Id = 13,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            Specializations = new List<SpecializationBaseViewModel>()
            {
                new SpecializationBaseViewModel() {Id = 17, Name = "Younger surgeon"},
                new SpecializationBaseViewModel() {Id = 18, Name = "Master surgeon"},
            }
        };
        return procedureReadViewModel;
    }

    private PagedList<Procedure> GetPagedProcedures()
    {
        var procedures = new List<Procedure>()
        {
            new Procedure()
            {
                Id = 1,
                Name = "haircut",
                Description = "haircut description",
                DurationInMinutes = 35
            },
            new Procedure()
            {
                Id = 2,
                Name = "ears cleaning",
                Description = "ears cleaning description",
                DurationInMinutes = 35,
                ProcedureSpecializations = new List<ProcedureSpecialization>()
                {
                    new ProcedureSpecialization()
                    {
                        ProcedureId = 2,
                        SpecializationId = 15,
                        Specialization = new Specialization() {Id = 15, Name = "Therapist"}
                    }
                }
            },
            new Procedure()
            {
                Id = 3,
                Name = "leg surgery",
                Description = "leg surgery description",
                DurationInMinutes = 35,
                ProcedureSpecializations = new List<ProcedureSpecialization>()
                {
                    new ProcedureSpecialization()
                    {
                        ProcedureId = 3,
                        SpecializationId = 17,
                        Specialization = new Specialization() {Id = 17, Name = "Younger surgeon"}
                    },
                    new ProcedureSpecialization()
                    {
                        ProcedureId = 3,
                        SpecializationId = 18,
                        Specialization = new Specialization() {Id = 18, Name = "Master surgeon"}
                    }
                }
            }
        };
        
        var pagedProcedures = new PagedList<Procedure>(procedures, 3, 1, 5);
        return pagedProcedures;
    }

    private PagedReadViewModel<ProcedureReadViewModel> GetPagedReadViewModel()
    {
        var procedureReadViewModels = new List<ProcedureReadViewModel>()
        {
            new ProcedureReadViewModel()
            {
                Id = 1,
                Name = "haircut",
                Description = "haircut description",
                DurationInMinutes = 35
            },
            new ProcedureReadViewModel()
            {
                Id = 2,
                Name = "ears cleaning",
                Description = "ears cleaning description",
                DurationInMinutes = 35,
                Specializations = new List<SpecializationBaseViewModel>()
                {
                    new SpecializationBaseViewModel() {Id = 15, Name = "Therapist"}
                }
            },
            new ProcedureReadViewModel()
            {
                Id = 3,
                Name = "leg surgery",
                Description = "leg surgery description",
                DurationInMinutes = 35,
                Specializations = new List<SpecializationBaseViewModel>()
                {
                    new SpecializationBaseViewModel() {Id = 17, Name = "Younger surgeon"},
                    new SpecializationBaseViewModel() {Id = 18, Name = "Master surgeon"}
                }
            }
        };
        
        var pagedReadViewModel = new PagedReadViewModel<ProcedureReadViewModel>()
            {
                CurrentPage = 1,
                Entities = procedureReadViewModels,
                HasNext = false,
                HasPrevious = false,
                PageSize = 5,
                TotalCount = 3,
                TotalPages = 1
            };
        return pagedReadViewModel;
    }
}