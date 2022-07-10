using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Moq;
using System.Linq.Expressions;

namespace Application.Test
{
    public class AppointmentServiceTests : IClassFixture<AppointmentServiceFixture>
    {
        public AppointmentServiceTests(AppointmentServiceFixture appointmentServiceFixture)
        {
            _appointmentServiceFixture = appointmentServiceFixture;
        }

        private readonly AppointmentServiceFixture _appointmentServiceFixture;

        [Fact]
        public async Task GetAppointmentsAsync_Appointments_ReturnsIEnumerableOfAppointment()
        {
            // Arrange
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_appointmentServiceFixture.MockListAppointments);

            // Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_appointmentServiceFixture.MockListAppointments, result);
        }

        [Fact]
        public async Task GetAppointmentsAsync_whenAppointmentsExist_thenReturn()
        {
            List<Appointment> emptyAppointments = new List<Appointment>();

            // Arrange
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(emptyAppointments);

            // Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService.GetAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_Appointment_ReturnsOkObjectResult()
        {
            //Arrange
            var id = 1;
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_appointmentServiceFixture.MockAppointment);

            //Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService
                .GetAsync(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_appointmentServiceFixture.MockAppointment, result);
        }

        [Fact]
        public async Task GetByIdAsync_whenAppointmentDoesNotExist_thanThrowNotFound()
        {
            //Arrange
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.GetAsync(10);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task CreateAsync_whenNormal_thanSuccessResult()
        {
            // Arrange
            var ids = new List<int> { 1, 2, 3, 4 };
            var animalId = 1;

            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointmentServiceFixture.MockProcedure);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_appointmentServiceFixture.MockUser);

            // Act
            await _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(
                _appointmentServiceFixture.MockAppointment,
                ids,
                ids,
                animalId);

            // Assert
            _appointmentServiceFixture.MockAppointmentRepository.Verify(x => x.InsertAsync(_appointmentServiceFixture.MockAppointment), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_whenProcedureListIsEmpty_thanSuccess()
        {
            //Arrange
            var emptyprocedureIds = new List<int>();
            var ids = new List<int> { 1, 2, 3, 4 };
            var animalId = 1;

            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointmentServiceFixture.MockProcedure);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_appointmentServiceFixture.MockUser);

            //Act
            var result =  _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentServiceFixture.MockAppointment,
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

            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointmentServiceFixture.MockProcedure);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_appointmentServiceFixture.MockUser);

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentServiceFixture.MockAppointment,
                ids, emptyUserIds, animalId);
            await result;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAsync_whenAppointmentExist_thanSuccess()
        {
            //Arrange
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(_appointmentServiceFixture.MockAppointment);

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Delete(
                    It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.DeleteAsync(_appointmentServiceFixture.MockAppointment.Id);
            await result;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAsync_whenAppointmentDoesNotExist_thanThrowNotFound()
        {
            //Arrange
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Delete(
                    It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.DeleteAsync(17);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task UpdateAsync_whenNormal_thanSuccess()
        {
            //Arrange
            _appointmentServiceFixture.MockAppointmentRepository
               .Setup(repo => repo.GetById(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                    .ReturnsAsync(_appointmentServiceFixture.existingAppointment);

            _appointmentServiceFixture.MockAppointmentRepository
            .Setup(repo => repo.Update(It.IsAny<Appointment>()))
            .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAsync(_appointmentServiceFixture.MockAppointment);
            await result;   

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateAsync_whenSomeAppointmentDontExist_thanThrowNotFound()
        {
            //Arrange
            _appointmentServiceFixture.MockAppointmentRepository
               .Setup(repo => repo.GetById(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
               .ReturnsAsync(() => null);

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAsync(_appointmentServiceFixture.MockAppointment);

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

           _appointmentServiceFixture.MockAppointmentProcedureRepository.Setup(
                repo => repo.GetAsync
                    (
                It.IsAny<Expression<Func<AppointmentProcedure, bool>>>(),
                It.IsAny<Func<IQueryable<AppointmentProcedure>, IOrderedQueryable<AppointmentProcedure>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .ReturnsAsync(_appointmentServiceFixture._appointmentProcedure);

            // Act
            await _appointmentServiceFixture.MockAppointmentEntityService
                .UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, procedureIds);

            // Assert
            _appointmentServiceFixture.MockAppointmentRepository.Verify(
                repo => repo.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdateAppointmentProceduresAsync_WhenSomeProcedureListEmpty_ThenUpdateIsCanceled()
        {
            //Act
            await _appointmentServiceFixture.MockAppointmentEntityService.UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, new List<int>());

            //Assert
            _appointmentServiceFixture.MockAppointmentProcedureRepository
                .Verify(repository => repository.InsertAsync(It.IsAny<AppointmentProcedure>()), Times.Never);
            _appointmentServiceFixture.MockAppointmentProcedureRepository
                .Verify(repository => repository.SaveChangesAsync(), Times.Never);
        }


        [Fact]
        public async Task UpdateAsync_WhenSomeUserListEmpty_ThenUpdateIsCanceled()
        {
            //Act
            await _appointmentServiceFixture.MockAppointmentEntityService
                .UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, new List<int>());

            //Assert
            _appointmentServiceFixture.MockAppointmentUserRepository
                .Verify(repository => repository.InsertAsync(It.IsAny<AppointmentUser>()), Times.Never);

            _appointmentServiceFixture.MockAppointmentUserRepository
                .Verify(repository => repository.SaveChangesAsync(), Times.Never);
        }

    }
}



 
