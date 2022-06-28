using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

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
                        It.Is<int>(specId => specId == id),
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
        public async Task GetSpecializationById_whenIdIsInvalid_thenThrowException()
        {
            int id = -1;

            Specialization returned = null;

            _fixture.MockRepository
                .Setup(repository =>
                    repository.GetById(
                        It.Is<int>(specId => specId == id),
                        It.IsAny<string>()))
                .ReturnsAsync(returned);


            var result =
                 _fixture.MockService.GetSpecializationByIdAsync(id);

            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }


        [Fact]
        public async Task GetAllSpecializations_whenResultNotEmpty_thenReturnSpecializations()
        {
            var expected = new List<Specialization>()
            {
                new Specialization() { Id = 0, Name = "Surgeon"},
                new Specialization() { Id = 1, Name = "Cleaner"}
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetAsync(
                    It.IsAny<Expression<Func<Specialization, bool>>>(),
                    It.IsAny<Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>?>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(new List<Specialization>()
            {
                new Specialization() { Id = 0, Name = "Surgeon"},
                new Specialization() { Id = 1, Name = "Cleaner"}
            });


            var result = await _fixture.MockService.GetAllSpecializationsAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
