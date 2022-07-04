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

        const string includeProperties = "ProcedureSpecializations.Procedure,ProcedureSpecializations,UserSpecializations.User";

        public SpecializationServiceTests(SpecializationServiceFixture fixture)
        {
            _fixture = fixture;            
        }

        [Fact]
        public async Task GetSpecializationById_whenIdIsCorrect_thenReturnSpecialization()
        {
            int id = 1;

            _fixture.MockRepository
                .Setup(repository =>
                    repository.GetById(
                        It.Is<int>(specId => specId == id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.Expected);


            var result = 
                await _fixture.MockService.GetSpecializationByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(_fixture.Expected.Name, result.Name);
            Assert.Equal(_fixture.Expected.Id, result.Id);
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
            Assert.IsType<List<Specialization>>(result);
        }

        [Fact]
        public async Task GetAllSpecializations_whenResultIsEmpty_thenReturnNothing()
        {
            var emptyList = new List<Specialization>();

            _fixture.MockRepository.Setup(repository =>
                repository.GetAsync(
                    It.IsAny<Expression<Func<Specialization, bool>>>(),
                    It.IsAny<Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>?>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(emptyList);

            var result = await _fixture.MockService.GetAllSpecializationsAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddSpecialization_whenDataIsCorrect_thenReturnCreated()
        {
            var specialization = new Specialization()
            {
                Id = 1,
                Name = "doctor"
            };

            _fixture.MockRepository.Setup(repository =>
                repository.InsertAsync(It.IsAny<Specialization>()))
            .Returns(Task.FromResult<object?>(null));

            var result = await _fixture.MockService.AddSpecializationAsync(specialization);

            Assert.NotNull(result);
            Assert.Equal(specialization, result);
        }

        [Fact]
        public async Task UpdateSpecialization_whenIdIsCorrect_thenExecute()
        {
            int id = 0;

            string includeProperties = "ProcedureSpecializations.Procedure,ProcedureSpecializations,UserSpecializations.User";

            var updatedSpecialization = new Specialization()
            {
                Id = 0,
                Name = "updatedName"
            };

            var specialization = new Specialization
            {
                Id = 0,
                Name = "oldName"
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(
                    It.Is<int>(specId => specId == id),
                    It.Is<string>(properties => properties == includeProperties)))
            .ReturnsAsync(specialization)
            .Verifiable();

            _fixture.MockRepository.Setup(repository =>
                repository.Update(It.IsAny<Specialization>()))
            .Verifiable();

            await _fixture.MockService.UpdateSpecializationAsync(id, updatedSpecialization);

            _fixture.MockRepository.Verify(
                repo => repo.GetById(id, includeProperties), Times.Once);

            _fixture.MockRepository.Verify(
                repo => repo.Update(specialization), Times.Once);
        }

        [Fact]
        public async Task UpdateSpecialization_whenIdIsIncorrect_thenThrowException()
        {
            int id = 40;

            string includeProperties = "ProcedureSpecializations.Procedure,ProcedureSpecializations,UserSpecializations.User";

            var updatedSpecialization = new Specialization()
            {
                Id = 40,
                Name = "updatedName"
            };

            Specialization specialization = null;

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(
                    It.Is<int>(specId => specId == id),
                    It.Is<string>(properties => properties == includeProperties)))
            .ReturnsAsync(specialization);

            var result = _fixture.MockService.UpdateSpecializationAsync(id, updatedSpecialization);

            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task DeleteSpecialization_whenSpecializationExists_thenExecute()
        {
            int id = 4;

            string includeProperties = "ProcedureSpecializations.Procedure,ProcedureSpecializations,UserSpecializations.User";

            var specializationToDelete = new Specialization
            {
                Id = id,
                Name = "cleaner"
            };

            _fixture.MockRepository.Setup(repository =>
                repository.Delete(It.Is<Specialization>(spec => specializationToDelete == spec)))
            .Verifiable();

            _fixture.MockRepository.Setup(repository => 
                repository.GetById(
                    It.Is<int>(specId => specId == id),
                    It.Is<string>(props => props == includeProperties)))
            .ReturnsAsync(specializationToDelete);

            await _fixture.MockService.DeleteSpecializationAsync(id);

            _fixture.MockRepository.Verify(
                method => method.GetById(id, includeProperties), Times.Once);

            _fixture.MockRepository.Verify(
                method => method.Delete(specializationToDelete), Times.Once); ;
        }

        [Fact]
        public async Task DeleteSpecialization_whenSpecializationIsntExists_thenThrowException()
        {
            int id = 40;

            Specialization notFound = null; 

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(
                    It.Is<int>(specId => specId == id),
                    It.Is<string>(props => props == includeProperties)))
            .ReturnsAsync(notFound);

            var result = _fixture.MockService.DeleteSpecializationAsync(id);

            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task RemoveProcedureFromSpecialization_whenSpecializationExists_thenExecute()
        {
            int specializationId = 2;
            int procedureId = 1;

            var relationshipToRemove =
                new ProcedureSpecialization { ProcedureId = procedureId, SpecializationId = specializationId };

            Specialization specialization = new Specialization
            {
                Id = specializationId,
                Name = "doctor",
                ProcedureSpecializations = new List<ProcedureSpecialization>()
                {
                    relationshipToRemove,
                    new ProcedureSpecialization { ProcedureId = 0, SpecializationId = specializationId }
                }
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(It.Is<int>(specId => specId == specializationId),
                    It.Is<string>(props => props == includeProperties)))
                .ReturnsAsync(specialization);

            _fixture.MockRepository.Setup(repository =>
                repository.Update(
                    It.Is<Specialization>(spec => spec == specialization)))
                .Verifiable();

            await _fixture.MockService.RemoveProcedureFromSpecialization(specializationId,procedureId);

            Assert.DoesNotContain(relationshipToRemove, specialization.ProcedureSpecializations);
            _fixture.MockRepository.Verify(method => method.Update(specialization), Times.Once);
        }

        [Fact]
        public async Task RemoveProcedureFromSpecialization_whenProcedureNotExists_thenThrowException()
        {
            int specializationId = 2;
            int procedureId = 1;

            Specialization specialization = new Specialization
            {
                Id = specializationId,
                Name = "doctor",
                ProcedureSpecializations = new List<ProcedureSpecialization>()
                {
                    new ProcedureSpecialization { ProcedureId = 0, SpecializationId = specializationId }
                }
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(It.Is<int>(specId => specId == specializationId),
                    It.Is<string>(props => props == includeProperties)))
                .ReturnsAsync(specialization);

            var result = _fixture.MockService
                .RemoveProcedureFromSpecialization(specializationId, procedureId);

            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task AddProcedureToSpecialization_whenSpecializationExists()
        {
            var specializationId = 2;

            var procedureId = 1;

            var expectedRelationship = new ProcedureSpecialization
            {
                SpecializationId = specializationId,
                ProcedureId = procedureId
            };

            Specialization specialization = new Specialization
            {
                Id = specializationId,
                Name = "doctor",
                ProcedureSpecializations = new List<ProcedureSpecialization>()
                {
                    new ProcedureSpecialization { ProcedureId = 0, SpecializationId = specializationId }
                }
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(It.Is<int>(specId => specId == specializationId),
                    It.Is<string>(props => props == includeProperties)))
                .ReturnsAsync(specialization);

            _fixture.MockRepository.Setup(repository =>
                repository.Update(
                    It.Is<Specialization>(spec => spec == specialization)))
                .Verifiable();

            await _fixture.MockService.AddProcedureToSpecialization(specializationId, procedureId);

            specialization.ProcedureSpecializations.Should().ContainEquivalentOf(expectedRelationship);

            _fixture.MockRepository.Verify(method => method.Update(specialization), Times.Once);
        }
    }
}
