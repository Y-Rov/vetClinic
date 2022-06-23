using Core.Entities;
using Core.Exceptions;
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

    [Fact]
    public async Task GetProcedureById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //  Arrange
        int id = 1;
        var procedure = new Procedure()
        {
            Id = id,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            ProcedureSpecializations = new List<ProcedureSpecialization>()
            {
                new ProcedureSpecialization() {ProcedureId = id, SpecializationId = 17},
                new ProcedureSpecialization() {ProcedureId = id, SpecializationId = 18},
            }
        };

        var procedureReadViewModel = new ProcedureReadViewModel()
        {
            Id = id,
            Name = "haircut",
            Description = "haircut description",
            DurationInMinutes = 35,
            Specializations = new List<SpecializationBaseViewModel>()
            {
                new SpecializationBaseViewModel() {Id = 17, Name = "Younger surgeon"},
                new SpecializationBaseViewModel() {Id = 17, Name = "Master surgeon"},
            }
        };

        _fixture.MockProcedureService
            .Setup(service =>
                service.GetByIdAsync(It.Is<int>(x => x == procedure.Id)))
            .ReturnsAsync(procedure);

        _fixture.MockProcedureReadViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<Procedure>(x => x == procedure)))
            .Returns(procedureReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(id);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, procedureReadViewModel);
    }

    [Fact]
    public async Task GetProcedureById_whenIdIsIncorrect_thenStatusCodeNotFoundReturned()
    {
        //  Arrange
        int id = 35;
        var procedure = new Procedure()
        {
            Id = 1,
            Name = "haircut",
            Description = "haircut description",
            DurationInMinutes = 35
        };

        var procedureReadViewModel = new ProcedureReadViewModel()
        {
            Id = 1,
            Name = "haircut",
            Description = "haircut description",
            DurationInMinutes = 35
        };

        _fixture.MockProcedureService
            .Setup(service =>
                service.GetByIdAsync(It.Is<int>(x => x == procedure.Id)))
            .ReturnsAsync(procedure);

        _fixture.MockProcedureReadViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<Procedure>(p => p == procedure)))
            .Returns(procedureReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(id);

        //  Assert
        Assert.NotNull(result);
        Assert.NotEqual(result, procedureReadViewModel);
    }

    [Fact]
    public async Task GetAll_whenProceduresListIsNotEmpty_thenStatusOkReturned()
    {
        //  Arrange
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

        _fixture.MockProcedureService
            .Setup(service =>
                service.GetAllProceduresAsync())
            .ReturnsAsync(procedures);

        _fixture.MockProcedureReadViewModelListMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Procedure>>(p => p.Equals(procedures))))
            .Returns(procedureReadViewModels);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync();

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, procedureReadViewModels);
    }

    [Fact]
    public async Task GetAll_whenProceduresListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var procedures = new List<Procedure>();

        var procedureReadViewModels = new List<ProcedureReadViewModel>();

        _fixture.MockProcedureService
            .Setup(service =>
                service.GetAllProceduresAsync())
            .ReturnsAsync(procedures);

        _fixture.MockProcedureReadViewModelListMapper
            .Setup(mapper =>
                mapper.Map(It.Is<IEnumerable<Procedure>>(p => p.Equals(procedures))))
            .Returns(procedureReadViewModels);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync();

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, procedureReadViewModels);
    }
    
    [Fact]
    public async Task Create_whenAllSpecializationsExist_thenStatusOkReturned()
    {
        //  Arrange
        var procedureCreateViewModel = new ProcedureCreateViewModel()
        {
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                1, 2
            }
        };

        var procedure = new Procedure()
        {
            Id = 0,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockCreateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureCreateViewModel>()))
            .Returns(procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockProcedureController.CreateAsync(procedureCreateViewModel);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }
    
    [Fact]
    public async Task Create_whenNotAllSpecializationsExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        var procedureCreateViewModel = new ProcedureCreateViewModel()
        {
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                1, 2
            }
        };

        var procedure = new Procedure()
        {
            Id = 0,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockCreateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureCreateViewModel>()))
            .Returns(procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result = _fixture.MockProcedureController.CreateAsync(procedureCreateViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Create_whenViewModelSpecListIsEmpty_thenStatusOkReturned()
    {
        //  Arrange
        var procedureCreateViewModel = new ProcedureCreateViewModel()
        {
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        var procedure = new Procedure()
        {
            Id = 0,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockCreateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureCreateViewModel>()))
            .Returns(procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockProcedureController.CreateAsync(procedureCreateViewModel);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }
    
    [Fact]
    public async Task Update_whenAllSpecializationsExist_thenStatusOkReturned()
    {
        //  Arrange
        var procedureUpdateViewModel = new ProcedureUpdateViewModel()
        {
            Id = 1,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                1, 2
            }
        };

        var procedure = new Procedure()
        {
            Id = 1,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockUpdateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureUpdateViewModel>()))
            .Returns(procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        //  Act
        await _fixture.MockProcedureController.UpdateAsync(procedureUpdateViewModel);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }

    [Fact]
    public async Task Update_whenNotAllSpecializationsExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        var procedureUpdateViewModel = new ProcedureUpdateViewModel()
        {
            Id = 1,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                1, 2
            }
        };

        var procedure = new Procedure()
        {
            Id = 1,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockUpdateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureUpdateViewModel>()))
            .Returns(procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();

        //  Act
        var result =  _fixture.MockProcedureController.UpdateAsync(procedureUpdateViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Update_whenProcedureDoesNotExist_thenStatusInternalErrorReturned()
    {
        //  Arrange
        var procedureUpdateViewModel = new ProcedureUpdateViewModel()
        {
            Id = -1,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
            SpecializationIds = new List<int>()
            {
                1, 2
            }
        };

        var procedure = new Procedure()
        {
            Id = -1,
            Name = "leg surgery",
            Description = "leg surgery description",
            DurationInMinutes = 35,
        };

        _fixture.MockUpdateProcedureMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<ProcedureUpdateViewModel>()))
            .Returns(procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result =  _fixture.MockProcedureController.UpdateAsync(procedureUpdateViewModel);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task Delete_whenProcedureExist_thenStatusOkReturned()
    {
        //  Arrange
        int id = 1;

        _fixture.MockProcedureService
            .Setup(service =>
                service.DeleteProcedureAsync(
                    It.IsAny<int>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        await _fixture.MockProcedureController.DeleteAsync(id);

        //  Assert
        _fixture.MockProcedureService.Verify();
    }
    
    [Fact]
    public async Task Delete_whenProcedureDoesNotExist_thenStatusNotFoundReturned()
    {
        //  Arrange
        int id = -1;

        _fixture.MockProcedureService
            .Setup(service =>
                service.DeleteProcedureAsync(
                    It.IsAny<int>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result =  _fixture.MockProcedureController.DeleteAsync(id);

        //  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }

}

/*_fixture.MockProcedureService
    .Setup(service =>
        service.CreateNewProcedureAsync(
            It.IsAny<Procedure>(),
            It.Is<IEnumerable<int>>(ids =>
                specializations.Select(s => s.Id).Intersect(ids).Count() != ids.Count() //if there are`nt spec with every id in ids
            )
        )
    ).Throws<NotFoundException>();
*/