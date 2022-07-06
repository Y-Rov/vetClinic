using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
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
        public async Task GetAppointmentsAsync_Appointments_Throw()
        {
            // Arrange
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync((IList<Appointment>)null);

            // Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService.GetAsync();

            // Assert
            Assert.Throws<NotFoundException>(() => result);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_whenAppointmentsListIsEmpty_thanReturnEmptyAppointmentsList()
        {
            //Arrange
            var emptyAppointments = new List<Appointment>();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Appointment, bool>>>(),
                    It.IsAny<Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(emptyAppointments);

            //Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService.GetAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(emptyAppointments, result);
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
            //Arrange
            var ids = new List<int> { 1, 2, 3, 4 };
            var animalId = 1;

            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointmentServiceFixture.MockProcedure);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_appointmentServiceFixture.MockUser);

            //Act
            await _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(
                _appointmentServiceFixture.MockAppointment,
                ids,
                ids,
                animalId);

            //Assert
            _appointmentServiceFixture.MockAppointmentRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
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
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentServiceFixture.MockAppointment,
                emptyprocedureIds, ids, animalId);
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

            var appointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
            };

            var existingAppointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a tail",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }

            };


            _appointmentServiceFixture.MockAppointmentRepository
               .Setup(repo => repo.GetById(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                    .ReturnsAsync(existingAppointment);

            _appointmentServiceFixture.MockAppointmentRepository
            .Setup(repo => repo.Update(It.IsAny<Appointment>()))
            .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAsync(appointment);

            //Assert
            Assert.NotNull(result);
        }



        [Fact]
        public async Task UpdateAsync_whenSomeAppointmentDontExist_thanThrowNotFound()
        {
            //Arrange

            var appointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
            };

            var existingAppointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a tail",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }

            };


            _appointmentServiceFixture.MockAppointmentRepository
               .Setup(repo => repo.GetById(
                        It.IsAny<int>(),
                        It.IsAny<string>()))
                    .ThrowsAsync(new DbUpdateException());

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAsync(appointment);

            //Assert
            await Assert.ThrowsAsync<DbUpdateException>(()=>result);
        }

        [Fact]
        public async Task UpdateAsync_whenSomeProcedureDontExist_thanThrowNotFound()
        {
            var appointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
            };

            //Arrange
            var procedureIds = new List<int>()
        {
            1, 2, 5
        };

            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Update(It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result =  _appointmentServiceFixture.MockAppointmentEntityService.UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, procedureIds);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);

        }



        [Fact]
        public async Task UpdateAsync_whenSomeUserDontExist_thanThrowNotFound()
        {
            var appointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
            };

            //Arrange
            var userIds = new List<int>()
        {
            1, 2, 5
        };

            _appointmentServiceFixture.MockUserEntityService
                .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Update(It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, userIds);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);

        }


        [Fact]
        public async Task UpdateAsync_whenSomeProcedureListEmpty()
        {
            var appointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
            };

            //Arrange
            var procedureIds = new List<int>();

            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointmentServiceFixture.MockProcedure);

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Update(It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, procedureIds);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);

        }


        [Fact]
        public async Task UpdateAsync_whenSomeUserListEmpty()
        {
            var appointment = new Appointment
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3,
                AppointmentProcedures = new List<AppointmentProcedure>()
                {
                    new AppointmentProcedure() {
                        AppointmentId = 1,
                        ProcedureId = 2,
                    }
                },

                AppointmentUsers = new List<AppointmentUser>()
                {
                    new AppointmentUser()
                    {
                        AppointmentId = 1,
                        UserId = 1,
                    }
                }
            };

            //Arrange
            var userIds = new List<int>();

            _appointmentServiceFixture.MockUserEntityService
                .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointmentServiceFixture.MockUser);

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Update(It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.UpdateAppointmentProceduresAsync(_appointmentServiceFixture.MockAppointment.Id, userIds);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);

        }

    }
}



 
