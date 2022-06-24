using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Test;

public class ProcedureServiceTests : IClassFixture<ProcedureServiceFixture>
{
    public ProcedureServiceTests(ProcedureServiceFixture fixture)
    {
        _fixture = fixture;
    }
    
    private readonly ProcedureServiceFixture _fixture;
    
    private readonly List<Procedure> _procedures = new()
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
    
    private readonly Procedure _procedure = new ()
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
    
    private readonly Specialization _specialization = new()
    {
        Id = 1, 
        Name = "Master surgeon"
    };
    
    private readonly IEnumerable<int> _specializationIds = new List<int>()
    {
        1, 2, 5, 6
    };
    
    private readonly IList<ProcedureSpecialization> _procedureSpecializations = new List<ProcedureSpecialization>()
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


    [Fact]
    public async Task GetAllProceduresAsync_whenProceduresListIsNotEmpty_thanReturnProceduresList()
    {
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Procedure, bool>>>(),
                It.IsAny<Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_procedures);
        
        //Act
        var result = await _fixture.MockProcedureService.GetAllProceduresAsync();
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(_procedures, result);
    }
    
    [Fact]
    public async Task GetAllProceduresAsync_whenProceduresListIsEmpty_thanReturnEmptyProceduresList()
    {
        //Arrange
        var emptyProcedures = new List<Procedure>();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Procedure, bool>>>(),
                It.IsAny<Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(emptyProcedures);
        
        //Act
        var result = await _fixture.MockProcedureService.GetAllProceduresAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(emptyProcedures, result);
    }
    
    [Fact]
    public async Task GetByIdAsync_whenProcedureExist_thanReturnTheProcedure()
    {
        //Arrange
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetById(
                It.Is<int>(x => x == _procedure.Id), 
                It.IsAny<string>()))
            .ReturnsAsync(_procedure);
        
        //Act
        var result = await _fixture.MockProcedureService.GetByIdAsync(_procedure.Id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_procedure, result);
    }
    
    [Fact]
    public async Task GetByIdAsync_whenProcedureDoesNotExist_thanThrowNotFound()
    {
        //Arrange
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(() => null);
        
        //Act
        var result = _fixture.MockProcedureService.GetByIdAsync(10);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task CreateAsync_whenNormal_thanSuccess()
    {
        //Arrange
        _fixture.MockSpecializationService
            .Setup(ss => ss.GetSpecializationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_specialization);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Procedure>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result =  _fixture.MockProcedureService.CreateNewProcedureAsync(_procedure, _specializationIds);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task CreateAsync_whenSomeSpecDontExist_thanThrowNotFound()
    {
        //Arrange
        _fixture.MockSpecializationService
            .Setup(ss => ss.GetSpecializationByIdAsync(It.IsAny<int>()))
            .Throws<NotFoundException>();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Procedure>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.CreateNewProcedureAsync(_procedure, _specializationIds);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task CreateAsync_whenSpecListIsEmpty_thanSuccess()
    {
        //Arrange
        var emptySpecializationIds = new List<int>();

        _fixture.MockSpecializationService
            .Setup(ss => ss.GetSpecializationByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(_specialization);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Procedure>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.CreateNewProcedureAsync(_procedure, emptySpecializationIds);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task UpdateAsync_whenNormal_thanSuccess()
    {
        //Arrange
        

        
        var specializationIds = new List<int>()
        {
            1, 2, 5
        };

        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<ProcedureSpecialization, bool>>>(),
                It.IsAny<Func<IQueryable<ProcedureSpecialization>, IOrderedQueryable<ProcedureSpecialization>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_procedureSpecializations);
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.Delete(It.IsAny<ProcedureSpecialization>()))
            .Verifiable();
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<ProcedureSpecialization>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();   
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.Update(It.IsAny<Procedure>()))
            .Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
            //.Throws<NotFoundException>();
        
        //Act
        var result =  _fixture.MockProcedureService.UpdateProcedureAsync(_procedure, specializationIds);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task UpdateAsync_whenSomeSpecDontExist_thanThrowNotFound()
    {
        //Arrange
        var exclusiveSpecializationIds = new List<int>()
        {
            1, 2, 5
        };

        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<ProcedureSpecialization, bool>>>(),
                It.IsAny<Func<IQueryable<ProcedureSpecialization>, IOrderedQueryable<ProcedureSpecialization>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_procedureSpecializations);
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.Delete(It.IsAny<ProcedureSpecialization>()))
            .Verifiable();

        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<ProcedureSpecialization>()))
            .Throws<DbUpdateException>(); 
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.Update(It.IsAny<Procedure>()))
            .Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.UpdateProcedureAsync(_procedure, exclusiveSpecializationIds);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => result);
    }
    
    [Fact]
    public async Task UpdateAsync_whenCreatingSpecListIsEmpty_thanSuccess()
    {
        //Arrange
        var emptySpecializationIds = new List<int>();

        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<ProcedureSpecialization, bool>>>(),
                It.IsAny<Func<IQueryable<ProcedureSpecialization>, IOrderedQueryable<ProcedureSpecialization>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(_procedureSpecializations);
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.Delete(It.IsAny<ProcedureSpecialization>()))
            .Verifiable();
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<ProcedureSpecialization>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();   
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.Update(It.IsAny<Procedure>()))
            .Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.UpdateProcedureAsync(_procedure, emptySpecializationIds);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task UpdateAsync_whenExistingSpecListIsEmpty_thanSuccess()
    {
        //Arrange

        var emptyProcedureSpecializations = new List<ProcedureSpecialization>();

        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<ProcedureSpecialization, bool>>>(),
                It.IsAny<Func<IQueryable<ProcedureSpecialization>, IOrderedQueryable<ProcedureSpecialization>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(emptyProcedureSpecializations);
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.Delete(It.IsAny<ProcedureSpecialization>()))
            .Verifiable();
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<ProcedureSpecialization>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();   
        
        _fixture.MockProcedureSpecializationRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.Update(It.IsAny<Procedure>()))
            .Verifiable();

        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.UpdateProcedureAsync(_procedure, _specializationIds);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task DeleteAsync_whenProcedureExist_thanSuccess()
    {
        //Arrange
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(_procedure);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.Delete(
                It.IsAny<Procedure>()))
            .Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.DeleteProcedureAsync(_procedure.Id);
        
        //Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task DeleteAsync_whenProcedureDoesNotExist_thanThrowNotFound()
    {
        //Arrange
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetById(
                It.IsAny<int>(), 
                It.IsAny<string>()))
            .ReturnsAsync(() => null);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.Delete(
                It.IsAny<Procedure>()))
            .Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.DeleteProcedureAsync(17);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>result);
    }
}