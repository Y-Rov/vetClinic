using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class AppointmentServiceTests : IClassFixture<AppointmentServiceFixture>, IDisposable
    {
        private readonly AppointmentServiceFixture _fixture;
        private bool _disposed;

        public AppointmentServiceTests(AppointmentServiceFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _fixture.MockAppointmentRepository.ResetCalls();
                _fixture.MockAppointmentProcedureRepository.ResetCalls();
                _fixture.MockAppointmentUserRepository.ResetCalls();
            }

            _disposed = true;
        }

        [Fact]
        public async Task GetAppointmentsAsync_Appointments_ReturnsIEnumerableOfAppointment()
        {
            // Arrange
            _fixture.MockAppointmentRepository
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_fixture.MockListAppointments);

            // Act
            var result = await _fixture.MockAppointmentEntityService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_fixture.MockListAppointments, result);
        }

        [Fact]
        public async Task GetAppointmentsAsync_whenAppointmentsExist_thenReturn()
        {
            List<Appointment> emptyAppointments = new List<Appointment>();

            // Arrange
            _fixture.MockAppointmentRepository
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(emptyAppointments);

            // Act
            var result = await _fixture.MockAppointmentEntityService.GetAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_Appointment_ReturnsOkObjectResult()
        {
            //Arrange
            var id = 1;
            _fixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.MockAppointment);

            //Act
            var result = await _fixture.MockAppointmentEntityService
                .GetAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_fixture.MockAppointment, result);
        }

        [Fact]
        public async Task GetByIdAsync_whenAppointmentDoesNotExist_thanThrowNotFound()
        {
            //Arrange
            _fixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Act
            var result = _fixture.MockAppointmentEntityService.GetAsync(10);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task CreateAsync_whenNormal_thanSuccessResult()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3, 4 };
            var animalId = 1;

            _fixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.MockProcedure);

            _fixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_fixture.MockUser);

            // Act
            await _fixture.MockAppointmentEntityService.CreateAsync(
                _fixture.MockAppointment,
                ids,
                ids,
                animalId);

            // Assert
            _fixture.MockAppointmentRepository.Verify(x => x.InsertAsync(_fixture.MockAppointment), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_whenProcedureListIsEmpty_thanSuccess()
        {
            //Arrange
            var emptyprocedureIds = new List<int>();
            var ids = new List<int> { 1, 2, 3, 4 };
            var animalId = 1;

            _fixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.MockProcedure);

            _fixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_fixture.MockUser);

            //Act
            var result =  _fixture.MockAppointmentEntityService.CreateAsync(_fixture.MockAppointment,
                emptyprocedureIds, ids, animalId);
            await result;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateAsync_whenUserListIsEmpty_thanSuccess()
        {
            // Arrange
            var emptyUserIds = new List<int>();
            var ids = new List<int> { 1, 2, 3, 4 };
            var animalId = 1;

            _fixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_fixture.MockProcedure);

            _fixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_fixture.MockUser);

            //Act
            var result = _fixture.MockAppointmentEntityService.CreateAsync(_fixture.MockAppointment,
                ids, emptyUserIds, animalId);
            await result;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAsync_whenAppointmentExist_thanSuccess()
        {
            //Arrange
            _fixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_fixture.MockAppointment);

            _fixture.MockAppointmentRepository
                .Setup(repo => repo.Delete(
                    It.IsAny<Appointment>()))
                .Verifiable();

            _fixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _fixture.MockAppointmentEntityService.DeleteAsync(_fixture.MockAppointment.Id);
            await result;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAsync_whenAppointmentDoesNotExist_thanThrowNotFound()
        {
            //Arrange
            _fixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _fixture.MockAppointmentRepository
                .Setup(repo => repo.Delete(
                    It.IsAny<Appointment>()))
                .Verifiable();

            _fixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _fixture.MockAppointmentEntityService.DeleteAsync(17);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateAsync_whenNormal_thanSuccess()
        {
            //Arrange
            _fixture.MockAppointmentRepository
               .Setup(repo => repo.GetById(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                    .ReturnsAsync(_fixture.existingAppointment);

            _fixture.MockAppointmentRepository
            .Setup(repo => repo.Update(It.IsAny<Appointment>()))
            .Verifiable();

            _fixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _fixture.MockAppointmentEntityService.UpdateAsync(_fixture.MockAppointment);
            await result;   

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_whenSomeAppointmentDontExist_thanThrowNotFound()
        {
            //Arrange
            _fixture.MockAppointmentRepository
               .Setup(repo => repo.GetById(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
               .ReturnsAsync(() => null);

            //Act
            var result = _fixture.MockAppointmentEntityService.UpdateAsync(_fixture.MockAppointment);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateProceduresAsync_whenProceduresExist_thanExecute()
        {
            //Arrange
            var procedureIds = new List<int>()
            {
                1, 2
            };

           _fixture.MockAppointmentProcedureRepository.Setup(
                repo => repo.GetAsync
                    (
                It.IsAny<Expression<Func<AppointmentProcedure, bool>>>(),
                It.IsAny<Func<IQueryable<AppointmentProcedure>, IOrderedQueryable<AppointmentProcedure>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppointmentProcedures);

            // Act
            await _fixture.MockAppointmentEntityService
                .UpdateAppointmentProceduresAsync(_fixture.MockAppointment.Id, procedureIds);

            // Assert
            _fixture.MockAppointmentRepository.Verify(
                repo => repo.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdateAppointmentProceduresAsync_WhenSomeProcedureListEmpty_ThenUpdateIsCanceled()
        {
            //Act
            await _fixture.MockAppointmentEntityService.UpdateAppointmentProceduresAsync(_fixture.MockAppointment.Id, new List<int>());

            //Assert
            _fixture.MockAppointmentProcedureRepository
                .Verify(repository => repository.InsertAsync(It.IsAny<AppointmentProcedure>()), Times.Never);
            _fixture.MockAppointmentProcedureRepository
                .Verify(repository => repository.SaveChangesAsync(), Times.Never);
        }


        [Fact]
        public async Task UpdateAsync_WhenSomeUserListEmpty_ThenUpdateIsCanceled()
        {
            //Act
            await _fixture.MockAppointmentEntityService
                .UpdateAppointmentProceduresAsync(_fixture.MockAppointment.Id, new List<int>());

            //Assert
            _fixture.MockAppointmentUserRepository
                .Verify(repository => repository.InsertAsync(It.IsAny<AppointmentUser>()), Times.Never);

            _fixture.MockAppointmentUserRepository
                .Verify(repository => repository.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAppointmentUsersAsync_WhenProceduresExist_ThanExecute()
        {
            //Arrange
            var userIds = new List<int>()
            {
                1, 2
            };

            _fixture.MockAppointmentUserRepository.Setup(
                    repo => repo.GetAsync(
                        It.IsAny<Expression<Func<AppointmentUser, bool>>>(),
                        It.IsAny<Func<IQueryable<AppointmentUser>, IOrderedQueryable<AppointmentUser>>>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(_fixture.AppointmentUsers);

            // Act
            await _fixture.MockAppointmentEntityService
                .UpdateAppointmentUsersAsync(_fixture.MockAppointment.Id, userIds);

            // Assert
            _fixture.MockAppointmentRepository.Verify(
                repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}



 
