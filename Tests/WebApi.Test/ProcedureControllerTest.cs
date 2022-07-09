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
    
    [Fact]
    public async Task GetProcedureById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //  Arrange
        _fixture.MockProcedureService
            .Setup(service =>
                service.GetByIdAsync(It.Is<int>(x => x == _fixture.Procedure.Id)))
            .ReturnsAsync(_fixture.Procedure);

        _fixture.MockProcedureReadViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.Is<Procedure>(x => x == _fixture.Procedure)))
            .Returns(_fixture.ExpectedProcedureReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(13);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _fixture.ExpectedProcedureReadViewModel);
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
                mapper.Map(It.Is<Procedure>(p => p == _fixture.Procedure)))
            .Returns(_fixture.ExpectedProcedureReadViewModel);

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
            .ReturnsAsync(_fixture.PagedProcedures);

        _fixture.MockPagedListMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Procedure>>()))
            .Returns(_fixture.PagedReadViewModel);

        //  Act
        var result = await _fixture.MockProcedureController.GetAsync(new ProcedureParameters());

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, _fixture.PagedReadViewModel);
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
            .Returns(_fixture.Procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();
        
        //  Act
        var result = _fixture.MockProcedureController.CreateAsync(_fixture.ProcedureCreateViewModel);

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
            .Returns(_fixture.Procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.CreateNewProcedureAsync(
                    It.IsAny<Procedure>(), 
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result = _fixture.MockProcedureController.CreateAsync(_fixture.ProcedureCreateViewModel);

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
            .Returns(_fixture.Procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Returns(Task.FromResult<object?>(null)).Verifiable();

        //  Act
        await _fixture.MockProcedureController.UpdateAsync(_fixture.ProcedureUpdateViewModel);

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
            .Returns(_fixture.Procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();

        //  Act
        var result =  _fixture.MockProcedureController.UpdateAsync(_fixture.ProcedureUpdateViewModel);

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
            .Returns(_fixture.Procedure);

        _fixture.MockProcedureService
            .Setup(service =>
                service.UpdateProcedureAsync(
                    It.IsAny<Procedure>(),
                    It.IsAny<IEnumerable<int>>()))
            .Throws<NotFoundException>();
        
        //  Act
        var result =  _fixture.MockProcedureController.UpdateAsync(_fixture.ProcedureUpdateViewModel);

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