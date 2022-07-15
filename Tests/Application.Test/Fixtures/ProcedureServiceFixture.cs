using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
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
        MockProcedureSpecializationRepository = fixture.Freeze<Mock<IProcedureSpecializationRepository>>();

        ExpectedProcedure = GetExpectedProcedure();
        ExpectedProcedures = GetExpectedProcedures();
        Specialization = GetSpecialization();
        SpecializationIds = GetSpecializationIds();
        ProcedureSpecializations = GetProcedureSpecializations();
        Parameters = GetParameters();
        PagedProcedures = GetPagedProcedures();
        
        MockProcedureService = new ProcedureService(
            MockProcedureRepository.Object,
            MockSpecializationService.Object,
            MockProcedureSpecializationRepository.Object,
            MockLoggerManager.Object);
    }
    
    public IProcedureService MockProcedureService { get; }
    public Mock<IProcedureRepository> MockProcedureRepository { get; }
    public Mock<ISpecializationService> MockSpecializationService { get; }
    public Mock<IProcedureSpecializationRepository> MockProcedureSpecializationRepository { get; }
    public Mock<ILoggerManager> MockLoggerManager { get; }
    
    public Procedure ExpectedProcedure { get; }
    public List<Procedure> ExpectedProcedures { get; }
    public Specialization Specialization { get; }
    public IEnumerable<int> SpecializationIds { get; }
    public IList<ProcedureSpecialization> ProcedureSpecializations { get; }
    public ProcedureParameters Parameters { get; }
    public PagedList<Procedure> PagedProcedures { get; }

    private Procedure GetExpectedProcedure()
    {
        var procedure = new Procedure()
        {
            Id = 18,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            ProcedureSpecializations = new List<ProcedureSpecialization>()
            {
                new ProcedureSpecialization()
                {
                    ProcedureId = 18,
                    SpecializationId = 17,
                    Specialization = new Specialization() {Id = 17, Name = "Younger surgeon"}
                },
                new ProcedureSpecialization()
                {
                    ProcedureId = 18,
                    SpecializationId = 18,
                    Specialization = new Specialization() {Id = 18, Name = "Master surgeon"}
                }
            }
        };
        return procedure;
    }
    private List<Procedure> GetExpectedProcedures() { 
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
        return procedures;
    }

    private Specialization GetSpecialization()
    {
        var specialization = new Specialization()
        {
            Id = 1, 
            Name = "Master surgeon"
        };
        return specialization;
    }

    private IEnumerable<int> GetSpecializationIds()
    {
        var specializationIds = new List<int>()
        {
            1, 2, 5, 6
        };
        return specializationIds;
    }

    private IList<ProcedureSpecialization> GetProcedureSpecializations()
    {
        var procedureSpecializations = new List<ProcedureSpecialization>()
        {
            new ProcedureSpecialization()
            {
                ProcedureId = 18,
                SpecializationId = 1,
                Specialization = new Specialization() {Id = 1, Name = "Younger surgeon"},

            },
            new ProcedureSpecialization()
            {
                ProcedureId = 18,
                SpecializationId = 2,
                Specialization = new Specialization() {Id = 2, Name = "Master surgeon"}
            },
            new ProcedureSpecialization()
            {
                ProcedureId = 18,
                SpecializationId = 5,
                Specialization = new Specialization() {Id = 5, Name = "Master surgeon"}
            },
            new ProcedureSpecialization()
            {
                ProcedureId = 18,
                SpecializationId = 8,
                Specialization = new Specialization() {Id = 8, Name = "Master surgeon"}
            }
        };
        return procedureSpecializations;
    }

    private ProcedureParameters GetParameters()
    {
        var parameters = new ProcedureParameters()
        {
            FilterParam = "hello",
            OrderByParam = "Cost",
            OrderByDirection = "desc",
            PageNumber = 1,
            PageSize = 5
        };
        return parameters;
    }

    private PagedList<Procedure> GetPagedProcedures()
    {
        var pagedProcedures = new PagedList<Procedure>(ExpectedProcedures, 3, 1, 5);
        return pagedProcedures;
    }
}