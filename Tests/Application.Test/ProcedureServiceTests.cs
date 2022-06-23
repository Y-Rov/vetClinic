using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
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
        //Arrange
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
        
        _fixture.MockProcedureRepository
            .Setup(repo => repo.GetAsync(
                It.IsAny<Expression<Func<Procedure, bool>>>(),
                It.IsAny<Func<IQueryable<Procedure>, IOrderedQueryable<Procedure>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
            .ReturnsAsync(procedures);
        
        //Act
        var result = await _fixture.MockProcedureService.GetAllProceduresAsync();
        
        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(procedures, result);
    }
}