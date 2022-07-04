using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels;
using Core.ViewModels.AppointmentsViewModel;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class AppointmentControllerFixture
    {
        public AppointmentControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            MockAppointmentService = fixture.Freeze<Mock<IAppointmentService>>();
            MockCreateAppointmentMapper = fixture.Freeze<Mock<IViewModelMapper<AppointmentCreateViewModel, Appointment>>>();
            MockAppointmentsViewModelMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>>>();
            MockAppointmentReadMapper = fixture.Freeze<Mock<IViewModelMapper < Appointment, AppointmentReadViewModel>>> ();
            MockAppointmentUpdateMapper = fixture.Freeze<Mock<IViewModelMapper<AppointmentUpdateViewModel, Appointment>>>();

            MockAppointmentController = new AppointmentController(
                MockAppointmentService.Object,
                MockCreateAppointmentMapper.Object,
                MockAppointmentsViewModelMapper.Object,
                MockAppointmentReadMapper.Object,
                MockAppointmentUpdateMapper.Object);
        }


        public AppointmentController MockAppointmentController { get; }
        public Mock<IAppointmentService> MockAppointmentService { get; }
        public Mock<IViewModelMapper<AppointmentCreateViewModel, Appointment>> MockCreateAppointmentMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>> MockAppointmentsViewModelMapper;
        public Mock<IViewModelMapper<Appointment, AppointmentReadViewModel>> MockAppointmentReadMapper { get; }
        public Mock<IViewModelMapper<AppointmentUpdateViewModel, Appointment>> MockAppointmentUpdateMapper { get; }

    }
}
