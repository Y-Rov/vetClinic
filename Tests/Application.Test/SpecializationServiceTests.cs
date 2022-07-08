using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class SpecializationServiceTests : IClassFixture<SpecializationServiceFixture>, IDisposable
    {
        private readonly SpecializationServiceFixture _fixture;
        private bool _disposed;

        const string includeProperties = "ProcedureSpecializations.Procedure,ProcedureSpecializations,UserSpecializations.User";

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockRepository.ResetCalls();
                _fixture.MockUserRepository.ResetCalls();
            }

            _disposed = true;
        }

        public SpecializationServiceTests(SpecializationServiceFixture fixture)
        {
            _fixture = fixture;            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task GetSpecializationById_whenIdIsCorrect_thenReturnSpecialization()
        {
            int id = 2;

            _fixture.MockRepository
                .Setup(repository =>
                    repository.GetById(
                        It.Is<int>(specId => specId == id),
                        It.IsAny<string>()))
                .ReturnsAsync(_fixture.Expected);


            var result = 
                await _fixture.MockService.GetSpecializationByIdAsync(id);

            Assert.NotNull(result);
            Assert.IsType<Specialization>(result);
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
                repository.GetAllAsync(
                    It.IsAny<SpecializationParameters>(),                    
                    It.IsAny<Expression<Func<Specialization, bool>>>(),
                    It.IsAny<Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>?>(),
                    It.IsAny<Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>>()))
            .ReturnsAsync(_fixture.ExpectedSpecializations);


            var result = await _fixture.MockService.GetAllSpecializationsAsync(_fixture.TestParameters);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<PagedList<Specialization>>(result);
        }

        [Fact]
        public async Task GetAllSpecializations_whenResultIsEmpty_thenReturnNothing()
        {
            var emptyPagedList = 
                    new PagedList<Specialization>(new List<Specialization>(), 0,1,4);

            _fixture.MockRepository.Setup(repository =>
                repository.GetAllAsync(
                    It.IsAny<SpecializationParameters>(),
                    It.IsAny<Expression<Func<Specialization, bool>>>(),
                    It.IsAny<Func<IQueryable<Specialization>, IOrderedQueryable<Specialization>>?>(),
                    It.IsAny<Func<IQueryable<Specialization>, IIncludableQueryable<Specialization, object>>>()))
            .ReturnsAsync(emptyPagedList);

            var result = 
                await _fixture.MockService.GetAllSpecializationsAsync(_fixture.TestParameters);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddSpecialization_whenDataIsCorrect_thenReturnCreated()
        {
            var result = await _fixture.MockService.AddSpecializationAsync(_fixture.Expected);

            Assert.NotNull(result);
            Assert.IsType<Specialization>(result);

            _fixture.MockRepository.Verify(repository => repository.InsertAsync(_fixture.Expected));
        }

        [Fact]
        public async Task UpdateSpecialization_whenIdIsCorrect_thenExecute()
        {
            int id = 0;

            var updatedSpecialization = new Specialization()
            {
                Id = 2,
                Name = "updatedSurgeon"
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(
                    It.Is<int>(specId => specId == id),
                    It.Is<string>(properties => properties == includeProperties)))
            .ReturnsAsync(_fixture.Expected)
            .Verifiable();

            _fixture.MockRepository.Setup(repository =>
                repository.Update(It.Is<Specialization>
                    (specialization => specialization == _fixture.Expected)));

            await _fixture.MockService.UpdateSpecializationAsync(id, updatedSpecialization);

            _fixture.MockRepository.Verify(
                repo => repo.GetById(id, includeProperties), Times.Once);

            _fixture.MockRepository.Verify(repository =>
                repository.Update(It.Is<Specialization>
                    (specialization => specialization == _fixture.Expected)),
                Times.Once);

            _fixture.MockRepository.ResetCalls();
        }

        [Fact]
        public async Task UpdateSpecialization_whenIdIsIncorrect_thenThrowException()
        {
            int id = 40;

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
            int id = 2;

            _fixture.MockRepository.Setup(repository =>
                repository.Delete(It.Is<Specialization>(spec => _fixture.Expected == spec)))
            .Verifiable();

            _fixture.MockRepository.Setup(repository => 
                repository.GetById(
                    It.Is<int>(specId => specId == id),
                    It.Is<string>(props => props == includeProperties)))
            .ReturnsAsync(_fixture.Expected);

            await _fixture.MockService.DeleteSpecializationAsync(id);

            _fixture.MockRepository.Verify(
                method => method.GetById(id, includeProperties), Times.Once);

            _fixture.MockRepository.Verify(
                method => method.Delete(_fixture.Expected), Times.Once); ;
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

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(It.Is<int>(specId => specId == specializationId),
                    It.Is<string>(props => props == includeProperties)))
                .ReturnsAsync(_fixture.Expected);

            _fixture.MockRepository.Setup(repository =>
                repository.Update(
                    It.Is<Specialization>(spec => spec == _fixture.Expected)))
                .Verifiable();

            await _fixture.MockService.RemoveProcedureFromSpecialization(specializationId,procedureId);

            Assert.DoesNotContain(relationshipToRemove, _fixture.Expected.ProcedureSpecializations);
            _fixture.MockRepository.Verify(method => method.Update(_fixture.Expected), Times.Once);

            _fixture.MockRepository.ResetCalls();
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
        public async Task AddProcedureToSpecialization_whenSpecializationExists_thenExecute()
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

        [Fact]
        public async Task AddUserToSpecialization_whenSpecializationExists_thenExecute()
        {
            var specializationId = 2;

            var userId = 1;

            var expectedRelationship = new UserSpecialization
            {
                SpecializationId = specializationId,
                UserId = userId
            };

            Specialization specialization = new Specialization
            {
                Id = specializationId,
                Name = "doctor",
                UserSpecializations = new List<UserSpecialization>()
                {
                    new UserSpecialization { UserId = 2, SpecializationId = specializationId }
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

             await _fixture.MockService.AddUserToSpecialization(specializationId, userId);

             specialization.UserSpecializations.Should().ContainEquivalentOf(expectedRelationship);

            _fixture.MockRepository.Verify(method => method.Update(specialization), Times.Once);

            _fixture.MockRepository.ResetCalls();
        }

        [Fact]
        public async Task AddUserToSpecialization_whenRelationshipExists_thenThrowException()
        {
            var specializationId = 2;

            var userId = 1;

            var expectedRelationship = new UserSpecialization
            {
                SpecializationId = specializationId,
                UserId = userId
            };

            Specialization specialization = new Specialization
            {
                Id = specializationId,
                Name = "doctor",
                UserSpecializations = new List<UserSpecialization>()
                {
                    new UserSpecialization { UserId = 2, SpecializationId = specializationId },
                    expectedRelationship
                }
            };

            _fixture.MockRepository.Setup(repository =>
                repository.GetById(It.Is<int>(specId => specId == specializationId),
                    It.Is<string>(props => props == includeProperties)))
                .ReturnsAsync(specialization);

            Task result = _fixture.MockService.AddUserToSpecialization(specializationId, userId);

            await Assert.ThrowsAsync<ArgumentException>(() => result);
        }
    }
}
