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

        private  readonly AppointmentServiceFixture _appointmentServiceFixture;
        

        private readonly Appointment _appointmentEntity = new Appointment()
        {
            Id = 1,
            Disease = "Pain",
            Date = DateTime.Now,    
            MeetHasOccureding = true
            

        };

        private readonly Procedure _procedureEntity = new Procedure()
        {
            Id = 1,
            Name = "haircut",
            Description = "haircut description",
            DurationInMinutes = 35


        };


        private readonly User _userEntity = new User()
        {
            FirstName = "Tom",
            LastName = "Smith",
            BirthDate = DateTime.Now,
            IsActive = true,
            ProfilePicture = { }


    };

        private readonly IEnumerable<int> _procedureIds = new List<int>()
    {
        1, 2, 5, 6
    };

        private readonly IEnumerable<int> _userIds = new List<int>()
    {
        1, 2, 5, 6
    };

        private  readonly List<Appointment> _appointmentList = new () { new Appointment()
    {
        Id = 1,
        Disease = "Pain",
        Date = DateTime.Now,
        MeetHasOccureding = true
    } };

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
                .ReturnsAsync(_appointmentList);


            // Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_appointmentList, result);
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
            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.GetById(
                    It.Is<int>(x => x == _appointmentEntity.Id),
                    It.IsAny<string>()))
                .ReturnsAsync(_appointmentEntity);

            //Act
            var result = await _appointmentServiceFixture.MockAppointmentEntityService
                .GetAsync(_appointmentEntity.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_appointmentEntity, result);
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
        public async Task CreateAsync_whenNormal_thanSuccess()
        {
            //Arrange
            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_procedureEntity);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_userEntity);


            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentEntity, _procedureIds, _userIds, 1);

            //Assert
            Assert.NotNull(result);
        }

        public async Task CreateAsync_whenSomeProcedureDontExist_thanThrowNotFound()
        {
            //Arrange
            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_userEntity);


            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentEntity, _procedureIds, _userIds, 1);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        public async Task CreateAsync_whenSomeUserDontExist_thanThrowNotFound()
        {
            //Arrange
            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(_procedureEntity);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .Throws<NotFoundException>();


            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentEntity, _procedureIds, _userIds, 1);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }


        [Fact]
        public async Task CreateAsync_whenProcedureListIsEmpty_thanSuccess()
        {
            //Arrange
            var emptyprocedureIds = new List<int>();

            //Arrange
            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_procedureEntity);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_userEntity);


            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentEntity, emptyprocedureIds, _userIds, 1);
            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task CreateAsync_whenГіукListIsEmpty_thanSuccess()
        {
            //Arrange
            var emptyUserIds = new List<int>();

            //Arrange
            _appointmentServiceFixture.MockProcedureEntityService
                .Setup(ss => ss.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_procedureEntity);

            _appointmentServiceFixture.MockUserEntityService
               .Setup(ss => ss.GetUserByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(_userEntity);


            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.InsertAsync(It.IsAny<Appointment>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.CreateAsync(_appointmentEntity, _procedureIds, emptyUserIds, 1);
            //Assert
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
                .ReturnsAsync(_appointmentEntity);

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.Delete(
                    It.IsAny<Appointment>()))
                .Verifiable();

            _appointmentServiceFixture.MockAppointmentRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var result = _appointmentServiceFixture.MockAppointmentEntityService.DeleteAsync(_appointmentEntity.Id);

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
                .ReturnsAsync(existingAppointment);

        _appointmentServiceFixture.MockAppointmentRepository
        .Setup(repo => repo.Update(It.IsAny<Appointment>()))
        .Throws<DbUpdateException>();

        _appointmentServiceFixture.MockAppointmentRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();

        //Act
        var result = await _appointmentServiceFixture.MockAppointmentEntityService.UpdateAsync(appointment);

        //Assert
       await Assert.NotNull(result);
    }
}


 
