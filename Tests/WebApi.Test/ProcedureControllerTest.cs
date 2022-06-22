using Core.Entities;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test;

public class ProcedureControllerTest : IClassFixture<ProcedureControllerFixture>
{
    public ProcedureControllerTest(ProcedureControllerFixture fixture)
    {
        _fixture = fixture;
    }

    private ProcedureControllerFixture _fixture;
    
    [Fact]
    public async Task GetProcedureById_whenIdIsCorrect_thenStatusCodeOkReturned()
    {
        //  Arrange

        int id = 1;
        var procedure = new Procedure()
        {
            Id = id,
            Name = "haircut",
            Description = "haircut description",
            DurationInMinutes = 35
        };

        var procedureViewModel = new ProcedureReadViewModel()
        {
            Id = id,
            Name = "haircut",
            Description = "haircut description",
            DurationInMinutes = 35
        };

        _fixture.MockProcedureService
            .Setup(service => 
                service.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(procedure);

        _fixture.MockProcedureReadViewModelMapper
            .Setup(mapper =>
                mapper.Map(It.IsAny<Procedure>()))
            .Returns(procedureViewModel);

        //  Act

        var result = await _fixture.MockProcedureController.GetAsync(id);

        //  Assert
        Assert.NotNull(result);
        Assert.Equal(result, procedureViewModel);
    }
}