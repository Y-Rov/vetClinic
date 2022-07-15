using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
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

    [Fact]
    public async Task GetAllProceduresAsync_whenProceduresListIsNotEmpty_thanReturnProceduresList()
    {
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetPaged(
                It.IsAny<ProcedureParameters>(),
                It.IsAny<Expression<Func<Procedure, bool>>>(),
                It.IsAny<Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(_fixture.PagedProcedures);
        
        //Act
        var result = await _fixture.MockProcedureService.GetAllProceduresAsync(_fixture.Parameters);
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(_fixture.PagedProcedures, result);
    }
    
    [Fact]
    public async Task GetAllProceduresAsync_whenProceduresListIsEmpty_thanReturnEmptyProceduresList()
    {
        //Arrange
        var emptyProcedures = new PagedList<Procedure>(new List<Procedure>(), 0, 0, 0);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetPaged(
                It.IsAny<ProcedureParameters>(),
                It.IsAny<Expression<Func<Procedure, bool>>>(),
                It.IsAny<Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>>(),
                It.IsAny<string>()
            ))
            .ReturnsAsync(emptyProcedures);
        
        //Act
        var result = await _fixture.MockProcedureService.GetAllProceduresAsync(_fixture.Parameters);
        
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
                It.Is<int>(x => x == _fixture.ExpectedProcedure.Id), 
                It.IsAny<string>()))
            .ReturnsAsync(_fixture.ExpectedProcedure);
        
        //Act
        var result = await _fixture.MockProcedureService.GetByIdAsync(_fixture.ExpectedProcedure.Id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_fixture.ExpectedProcedure, result);
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
            .ReturnsAsync(_fixture.Specialization);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Procedure>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result =  _fixture.MockProcedureService.CreateNewProcedureAsync(_fixture.ExpectedProcedure, _fixture.SpecializationIds);
        
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
        var result = _fixture.MockProcedureService.CreateNewProcedureAsync(_fixture.ExpectedProcedure, _fixture.SpecializationIds);
        
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
            .ReturnsAsync(_fixture.Specialization);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.InsertAsync(It.IsAny<Procedure>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.CreateNewProcedureAsync(_fixture.ExpectedProcedure, emptySpecializationIds);
        
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
            .ReturnsAsync(_fixture.ProcedureSpecializations);
        
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
        var result =  _fixture.MockProcedureService.UpdateProcedureAsync(_fixture.ExpectedProcedure, specializationIds);
        
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
            .ReturnsAsync(_fixture.ProcedureSpecializations);
        
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
        var result = _fixture.MockProcedureService.UpdateProcedureAsync(_fixture.ExpectedProcedure, exclusiveSpecializationIds);
        
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
            .ReturnsAsync(_fixture.ProcedureSpecializations);
        
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
        var result = _fixture.MockProcedureService.UpdateProcedureAsync(_fixture.ExpectedProcedure, emptySpecializationIds);
        
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
        var result = _fixture.MockProcedureService.UpdateProcedureAsync(_fixture.ExpectedProcedure, _fixture.SpecializationIds);
        
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
            .ReturnsAsync(_fixture.ExpectedProcedure);
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.Delete(
                It.IsAny<Procedure>()))
            .Verifiable();
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();
        
        //Act
        var result = _fixture.MockProcedureService.DeleteProcedureAsync(_fixture.ExpectedProcedure.Id);
        
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