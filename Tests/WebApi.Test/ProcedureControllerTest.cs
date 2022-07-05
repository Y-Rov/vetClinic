using System.Configuration;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.CommentViewModels;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test;

public class ProcedureControllerTest : IClassFixture<ProcedureControllerFixture>
{
    public ProcedureControllerTest(ProcedureControllerFixture fixture)
    {
        _fixture = fixture;
    }

    private readonly ProcedureControllerFixture _fixture;
    
    private readonly Procedure _procedure = new Procedure()
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
    
    private readonly ProcedureCreateViewModel _procedureCreateViewModel = new ()
    {
        Name = "leg surgery",
        Description = "leg surgery description",
        DurationInMinutes = 35,
        SpecializationIds = new List<int>()
        {
            17, 18
        }
    };
    
    private readonly ProcedureUpdateViewModel _procedureUpdateViewModel = new ProcedureUpdateViewModel()
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
    
    private readonly ProcedureReadViewModel _procedureReadViewModel = new ProcedureReadViewModel()
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
    
    private static readonly List<Procedure> _procedures = new List<Procedure>()
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

    private readonly PagedList<Procedure> _pagedProcedures = new PagedList<Procedure>(_procedures, 3, 1, 5);

    private readonly PagedReadViewModel<ProcedureReadViewModel> _pagedReadViewModel =
        new PagedReadViewModel<ProcedureReadViewModel>()
        {
            CurrentPage = 1,
            Entities = _procedureReadViewModels,
            HasNext = false,
            HasPrevious = false,
            PageSize = 5,
            TotalCount = 3,
            TotalPages = 1
        };

    private static readonly IEnumerable<ProcedureReadViewModel> _procedureReadViewModels = new List<ProcedureReadViewModel>()
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

    [Fact]
    public async Task GetProcedureById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //  Arrange
        _fixture.MockProcedureService
            .Setup(service =>
                service.GetByIdAsync(It.Is<int>(x => x == _procedure.Id)))
            .ReturnsAsync(_procedure);

        _fixture.MockProcedureReadViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<Procedure>(x => x == _procedure)))
            .Returns(_procedureReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(13);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _procedureReadViewModel);
    }

    [Fact]
    public async Task GetProcedureById_whenIdIsIncorrect_thenStatusCodeNotFoundReturned()
    {
        //  Arrange
        int id = 35;

        _fixture.MockProcedureService
            .Setup(service =>
                service.GetByIdAsync(It.IsAny<int>()))
            .Throws<NotFoundException>();

        _fixture.MockProcedureReadViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<Procedure>(p => p == _procedure)))
            .Returns(_procedureReadViewModel);

        //  Act
        var result =  _fixture.MockProcedureController.GetAsync(id);

        //  Assert
        Assert.NotNull(result);
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

    [Fact]
    public async Task GetAll_whenProceduresListIsNotEmpty_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockProcedureService
            .Setup(service =>
                service.GetAllProceduresAsync(It.IsAny<ProcedureParameters>()))
            .ReturnsAsync(_pagedProcedures);

        _fixture.MockPagedListMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Procedure>>()))
            .Returns(_pagedReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(new ProcedureParameters());

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _pagedReadViewModel);
    }

    [Fact]
    public async Task GetAll_whenProceduresListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var emptyPagedProcedures = new PagedList<Procedure>(new List<Procedure>(), 0, 0, 0);

        var emptyPagedReadViewModel = new PagedReadViewModel<ProcedureReadViewModel>();

        _fixture.MockProcedureService
            .Setup(service =>
                service.GetAllProceduresAsync(It.IsAny<ProcedureParameters>()))
            .ReturnsAsync(emptyPagedProcedures);

        _fixture.MockPagedListMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Procedure>>()))
            .Returns(emptyPagedReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(It.IsAny<ProcedureParameters>());

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, emptyPagedReadViewModel);
    }
    
    [Fact]
    public async Task Create_whenAllSpecializationsExist_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockCreateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureCreateViewModel>()))
            .Returns(_procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        var result = _fixture.MockProcedureController.CreateAsync(_procedureCreateViewModel);

        //  Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task Create_whenNotAllSpecializationsExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockCreateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureCreateViewModel>()))
            .Returns(_procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result = _fixture.MockProcedureController.CreateAsync(_procedureCreateViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Create_whenViewModelSpecListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var procedureCreateViewModelWithoutSpec = new ProcedureCreateViewModel()
        {
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        var procedureWithoutSpec = new Procedure()
        {
            Id = 0,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockCreateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureCreateViewModel>()))
            .Returns(procedureWithoutSpec);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockProcedureController.CreateAsync(procedureCreateViewModelWithoutSpec);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }
    
    [Fact]
    public async Task Update_whenAllSpecializationsExist_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockUpdateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureUpdateViewModel>()))
            .Returns(_procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        //  Act
        await _fixture.MockProcedureController.UpdateAsync(_procedureUpdateViewModel);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }

    [Fact]
    public async Task Update_whenNotAllSpecializationsExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockUpdateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureUpdateViewModel>()))
            .Returns(_procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();

        //  Act
        var result =  _fixture.MockProcedureController.UpdateAsync(_procedureUpdateViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Update_whenProcedureDoesNotExist_thenStatusInternalErrorReturned()
    {
        //  Arrange
        _fixture.MockUpdateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureUpdateViewModel>()))
            .Returns(_procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result =  _fixture.MockProcedureController.UpdateAsync(_procedureUpdateViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Delete_whenProcedureExist_thenStatusOkReturned()
    {
        //  Arrange
        _fixture.MockProcedureService
            .Setup(service =>
                service.DeleteProcedureAsync(
                    It.IsAny<int>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockProcedureController.DeleteAsync(1);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }
    
    [Fact]
    public async Task Delete_whenProcedureDoesNotExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        _fixture.MockProcedureService
            .Setup(service =>
                service.DeleteProcedureAsync(
                    It.IsAny<int>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result =  _fixture.MockProcedureController.DeleteAsync(65);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

}