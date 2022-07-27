using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.AppointmentsViewModel;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class AppointmentControllerTests : IClassFixture<AppointmentControllerFixture>, IDisposable
    {
        private readonly AppointmentControllerFixture _fixture;
        private bool _disposed;

        public AppointmentControllerTests(AppointmentControllerFixture fixture)
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
                _fixture.MockAppointmentService.ResetCalls();
            }

            _disposed = true;
        }

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
                .Returns(_fixture.MockAppointmentReadViewModel);

            //  Act
            var result = await _fixture.MockAppointmentController.GetAsync(3);

            //  Assert
            Assert.NotNull(result);
            Assert.IsType<AppointmentReadViewModel?>(result);
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
                .Returns(_fixture.MockAppointmentReadViewModel);

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
                    service.GetAsync(It.IsAny<AppointmentParameters>()))
                .ReturnsAsync(_fixture.MockAppointments);

            _fixture.MockPagedListMapper.Setup(mapper =>
                mapper.Map(It.IsAny<PagedList<Appointment>>()))
            .Returns(_fixture.MockAppointmentReadViewModels);

            //  Act
            var result = await _fixture.MockAppointmentController.GetAsync(_fixture.MockParameters);

            //  Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Entities);
            Assert.IsType<PagedReadViewModel<AppointmentReadViewModel>>(result);
        }

        [Fact]
        public async Task GetAll_whenAppointmentsListIsEmpty_thenStatusOkReturned()
        {
            //  Arrange
            var emptyAppointments = 
                new PagedList<Appointment>(new List<Appointment>(), 0, 1, 4);

            var emptyAppointmentReadViewModels = new PagedReadViewModel<AppointmentReadViewModel>()
            {
                Entities = new List<AppointmentReadViewModel>(),
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 2,
                TotalCount = 0,
                HasNext = true,
                HasPrevious = false
            };

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.GetAsync(_fixture.MockParameters))
                .ReturnsAsync(emptyAppointments);

            _fixture.MockPagedListMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<PagedList<Appointment>>()))
                .Returns(emptyAppointmentReadViewModels);

            //  Act
            var result = await _fixture.MockAppointmentController.GetAsync(_fixture.MockParameters);

            //  Assert
            Assert.NotNull(result);
            Assert.Empty(result.Entities);
            Assert.IsType<PagedReadViewModel<AppointmentReadViewModel>>(result);
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
                        It.IsAny<IEnumerable<int>>(),
                        It.IsAny<IEnumerable<int>>(),
                        It.IsAny<int>())
                        )
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //  Act
            var result = _fixture.MockAppointmentController.PostAsync(_fixture.MockAppointmentCreateViewModel);

            await result;
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
                        It.IsAny<IEnumerable<int>>(),
                        It.IsAny<IEnumerable<int>>(),
                        It.IsAny<int>()))
                .Throws<NotFoundException>();

            //  Act
            var result = _fixture.MockAppointmentController.PostAsync(_fixture.MockAppointmentCreateViewModel);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }

        [Fact]
        public async Task Create_whenViewModelProcedureListIsEmpty_thenStatusOkReturned()
        {
            //  Arrange
            _fixture.MockCreateAppointmentMapper
                .Setup(mapper =>
                    mapper.Map(It.IsAny<AppointmentCreateViewModel>()))
                .Returns(_fixture.appointmentWithoutProcedure);

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.CreateAsync(
                       It.IsAny<Appointment>(),
                        It.IsAny<IEnumerable<int>>(),
                        It.IsAny<IEnumerable<int>>(),
                        It.IsAny<int>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            await _fixture.MockAppointmentController.PostAsync(_fixture.MockAppointmentCreateViewModel);

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
                        It.IsAny<Appointment>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();
            
            _fixture.MockAppointmentService
               .Setup(service =>
                   service.UpdateAppointmentProceduresAsync(
                       It.IsAny<int>(),
                       It.IsAny<IEnumerable<int>>()))
               .Returns(Task.FromResult<object?>(null)).Verifiable();

            _fixture.MockAppointmentService
               .Setup(service =>
                   service.UpdateAppointmentUsersAsync(
                       It.IsAny<int>(),
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
                       It.IsAny<Appointment>()))
               .Returns(Task.FromResult<object?>(null)).Verifiable();

            _fixture.MockAppointmentService
                .Setup(service =>
                    service.UpdateAppointmentProceduresAsync(
                        It.IsAny<int>(),
                        It.IsAny<IEnumerable<int>>()))
                .Throws<NotFoundException>();

            _fixture.MockAppointmentService
              .Setup(service =>
                  service.UpdateAppointmentUsersAsync(
                      It.IsAny<int>(),
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
                        It.IsAny<Appointment>()))
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
    }
}
