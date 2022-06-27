using Application.Test.Fixtures;
using Core.Entities;
using Moq;

namespace Application.Test
{
    public class SpecializationServiceTests : IClassFixture<SpecializationServiceFixture>
    {
        private readonly SpecializationServiceFixture _fixture;
        public SpecializationServiceTests(SpecializationServiceFixture fixture)
        {
            _fixture = fixture;            
        }

        [Fact]
        public async Task GetSpecializationById_whenIdIsCorrect_thenReturnSpecialization()
        {
            int id = 1;
            var expected = new Specialization
            {
                Id = id,
                Name = "surgeon"
            };

            _fixture.MockRepository
                .Setup(repository =>
                    repository.GetById(
                        It.Is<int>(id => id == id),
                        It.IsAny<string>()))
                .ReturnsAsync(new Specialization
                {
                    Id = id,
                    Name = "surgeon"
                });


            var result = 
                await _fixture.MockService.GetSpecializationByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.Id, result.Id);
        }

        [Fact]
        public async Task GetAllSpecializations_whenResultNotEmpty_thenReturnSpecializations()
        {
               
        }
    }
}
