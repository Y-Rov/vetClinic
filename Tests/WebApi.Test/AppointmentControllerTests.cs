using Core.Entities;
using Core.Exceptions;
using Core.ViewModels;
using Core.ViewModels.AppointmentsViewModel;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class AppointmentControllerTests : IClassFixture<AppointmentControllerFixture>
    {
        public AppointmentControllerTests(AppointmentControllerFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly AppointmentControllerFixture _fixture;


        [Fact]
        public async Task GetAppointmentById_whenIdIsCorrect_thenStatusCodeOkReturned()
        {
            //  Arrange
            _fixture.MockAppointmentService
                .Setup(service =>
                    service.GetAsync(It.Is<int>(x => x == _fixture.MockAppointment.Id)))
                .ReturnsAsync(_fixture.MockAppointment);

            _fixture.MockAppointmentReadMapper
                .Setup(mapper =>
                    mapper.Map(It.Is<Appointment>(x => x == _fixture.MockAppointment)))
                .Returns(_fixture.MockAppointmentReadViewModels);

            //  Act
            var result = await _fixture.MockAppointmentController.GetAsync(3);

            //  Assert
            Assert.NotNull(result);
            Assert.Equal(result, _fixture.MockAppointmentReadViewModels);
        }

        [Fact]
        public async Task GetAppointmentById_whenIdIsIncorrect_thenStatusCodeNotFoundReturned()
        {
            //  Arrange
            int id = 23;

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.GetAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            _fixture.MockAppointmentReadMapper
                .Setup(mapper =>
                    mapper.Map(It.Is<Appointment>(p => p == _fixture.MockAppointment)))
                .Returns(_fixture.MockAppointmentsViewModelMapper);

            //  Act
            var result = _fixture.MockAppointmentController.GetAsync(id);

            //  Assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }


        [Fact]
        public async Task GetAll_whenAppointmentsListIsNotEmpty_thenStatusOkReturned()
        {
            //  Arrange
            _fixture.MockAppointmentService
                .Setup(service =>
                    service.GetAsync())
                .ReturnsAsync(_fixture.MockAppointments);

            _fixture.MockAppointmentsViewModelMapper
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<Appointment>>(p => p.Equals(_fixture.MockAppointments))))
                .Returns(_appointmentReadViewModels);

            //  Act
            var result = await _fixture.MockAppointmentController.GetAsync();

            //  Assert
            Assert.NotNull(result);
            Assert.Equal(result, _appointmentReadViewModels);
        }

        [Fact]
        public async Task GetAll_whenAppointmentsListIsEmpty_thenStatusOkReturned()
        {
            //  Arrange
            var emptyAppointments = new List<Appointment>();

            var emptyAppointmentReadViewModels = new List<AppointmentReadViewModel>();

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.GetAsync())
                .ReturnsAsync(emptyAppointments);

            _fixture.MockAppointmentReadMapper
                .Setup(mapper =>
                    mapper.Map(It.Is<IEnumerable<Appointment>>(p => p.Equals(emptyAppointments))))
                .Returns(emptyAppointmentReadViewModels);

            //  Act
            var result = await _fixture.MockAppointmentController.GetAsync();

            //  Assert
            Assert.NotNull(result);
            Assert.Equal(result, emptyAppointmentReadViewModels);
        }

        [Fact]
        public async Task Create_whenAllProceduresAndUsersAndAnimalExist_thenStatusOkReturned()
        {
            //  Arrange
            _fixture.MockCreateAppointmentMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentCreateViewModel>()))
                .Returns(_fixture.MockAppointment);

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.CreateAsync(
                        It.IsAny<Appointment>(),
                        It.IsAny<IEnumerable<int>>()))

                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            var result = _fixture.MockAppointmentController.PostAsync(_fixture.MockAppointmentCreateViewModel);

            //  Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_whenNotAllProceduresAndUsersAndAnimalExist_thenStatusNotFoundReturned()
        {
            //  Arrange
            _fixture.MockCreateAppointmentMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentCreateViewModel>()))
                .Returns(_fixture.MockAppointment);

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.CreateAsync(
                        It.IsAny<Appointment>(),
                        It.IsAny<IEnumerable<int>>()))
                .Throws<NotFoundException>();

            //  Act
            var result = _fixture.MockAppointmentController.CreateAsync(_fixture.MockAppointmentCreateViewModel);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task Create_whenViewModelProcedureListIsEmpty_thenStatusOkReturned()
        {
            //  Arrange
            var appointmentCreateViewModelWithoutProcedure = new AppointmentCreateViewModel()
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3
            };

            var appointmentWithoutProcedure = new Appointment()
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

            _fixture.MockCreateAppointmentMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentCreateViewModel>()))
                .Returns(appointmentWithoutProcedure);

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.CreateAsync(
                        It.IsAny<Appointment>(),
                        It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            await _fixture.MockAppointmentController.PostAsync(appointmentCreateViewModelWithoutProcedure);

            //  Assert
            _fixture.MockAppointmentService.Verify();
        }

        [Fact]
        public async Task Update_whenAllProcedureAndUserExist_thenStatusOkReturned()
        {
            //  Arrange
            _fixture.MockAppointmentUpdateMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentUpdateViewModel>()))
                .Returns(_fixture.MockAppointment);

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.UpdateAsync(
                        It.IsAny<Appointment>(),
                        It.IsAny<IEnumerable<int>>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            
            _fixture.MockAppointmentService
               .Setup(service =>
                   service.UpdateAppointmentProceduresAsync(
                       It.IsAny<Procedure>(),
                       It.IsAny<IEnumerable<int>>()))
               .Returns(Task.FromResult<object?>(null)).Verifiable();

            _fixture.MockAppointmentService
               .Setup(service =>
                   service.UpdateAppointmentUsersAsync(
                       It.IsAny<User>(),
                       It.IsAny<IEnumerable<int>>()))
               .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            await _fixture.MockAppointmentController.PutAsync(_fixture.MockAppointmentUpdateViewModel);

            //  Assert
            _fixture.MockAppointmentService.Verify();
        }


        [Fact]
        public async Task Update_whenNotAllProcedureExist_thenStatusNotFoundReturned()
        {
            //  Arrange
            _fixture.MockAppointmentUpdateMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentUpdateViewModel>()))
                .Returns(_fixture.MockAppointment);

            _fixture.MockAppointmentService
               .Setup(service =>
                   service.UpdateAsync(
                       It.IsAny<Appointment>(),
                       It.IsAny<IEnumerable<int>>()))
               .Returns(Task.FromResult<object?>(null)).Verifiable();

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.UpdateAppointmentProceduresAsync(
                        It.IsAny<Procedure>(),
                        It.IsAny<IEnumerable<int>>()))
                .Throws<NotFoundException>();

            _fixture.MockAppointmentService
              .Setup(service =>
                  service.UpdateAppointmentUsersAsync(
                      It.IsAny<User>(),
                      It.IsAny<IEnumerable<int>>()))
              .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            var result = _fixture.MockAppointmentController.PutAsync(_fixture.MockAppointmentUpdateViewModel);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }


        [Fact]
        public async Task Update_whenAppointmentDoesNotExist_thenStatusErrorReturned()
        {
            //  Arrange
            _fixture.MockAppointmentUpdateMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentUpdateViewModel>()))
                .Returns(_fixture.MockAppointment);

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.UpdateAsync(
                        It.IsAny<Appointment>(),
                        It.IsAny<IEnumerable<int>>()))
                .Throws<NotFoundException>();

            //  Act
            var result = _fixture.MockAppointmentController.PutAsync(_fixture.MockAppointmentUpdateViewModel);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }


        [Fact]
        public async Task Delete_whenAppointmentExist_thenStatusOkReturned()
        {
            //  Arrange
            _fixture.MockAppointmentService
                .Setup(service =>
                    service.DeleteAsync(
                        It.IsAny<int>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            await _fixture.MockAppointmentController.DeleteAsync(1);

            //  Assert
            _fixture.MockAppointmentService.Verify();
        }

        [Fact]
        public async Task Delete_whenAppointmentDoesNotExist_thenStatusNotFoundReturned()
        {
            //  Arrange
            _fixture.MockAppointmentService
                .Setup(service =>
                    service.DeleteAsync(
                        It.IsAny<int>()))
                .Throws<NotFoundException>();

            //  Act
            var result = _fixture.MockAppointmentController.DeleteAsync(48);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

    };
}

