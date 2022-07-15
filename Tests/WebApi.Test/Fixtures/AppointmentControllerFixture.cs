using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using Core.ViewModels.AppointmentsViewModel;
using Core.ViewModels.ProcedureViewModels;
using Core.ViewModels.SpecializationViewModels;
using Core.ViewModels.User;
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

            MockAppointment = GenerateAppointment();
            MockAppointmentCreateViewModel = GenerateAppointmentCreateViewModel();
            MockAppointmentUpdateViewModel = GenerateAppointmentUpdateViewModel();
            MockAppointmentReadViewModel = GenerateAppointmentReadViewModel();
            MockAppointments = GenerateAppointments();
            MockAppointmentReadViewModels = GenerateAppointmentReadViewModels();

            MockAppointmentService = fixture.Freeze<Mock<IAppointmentService>>();
            MockCreateAppointmentMapper = fixture.Freeze<Mock<IViewModelMapper<AppointmentCreateViewModel, Appointment>>>();
            MockAppointmentsViewModelMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>>>();
            MockAppointmentReadMapper = fixture.Freeze<Mock<IViewModelMapper<Appointment, AppointmentReadViewModel>>>();
            MockAppointmentUpdateMapper = fixture.Freeze<Mock<IViewModelMapper<AppointmentUpdateViewModel, Appointment>>>();
            MockViewModelIEnumMapper = fixture.Freeze <Mock<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AppointmentReadViewModel>>>>();

            MockAppointmentController = new AppointmentController(
                MockAppointmentService.Object,
                MockCreateAppointmentMapper.Object,
                MockAppointmentsViewModelMapper.Object,
                MockAppointmentReadMapper.Object,
                MockAppointmentUpdateMapper.Object,
                MockViewModelIEnumMapper.Object
                );
        }


        public AppointmentController MockAppointmentController { get; }
        public Mock<IAppointmentService> MockAppointmentService { get; }
        public Mock<IViewModelMapper<AppointmentCreateViewModel, Appointment>> MockCreateAppointmentMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentReadViewModel>>> MockAppointmentsViewModelMapper;
        public Mock<IViewModelMapper<Appointment, AppointmentReadViewModel>> MockAppointmentReadMapper { get; }
        public Mock<IViewModelMapper<AppointmentUpdateViewModel, Appointment>> MockAppointmentUpdateMapper { get; }
        public Mock<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AppointmentReadViewModel>>> MockViewModelIEnumMapper { get; }

    public Appointment MockAppointment { get; set; }
        public AppointmentCreateViewModel MockAppointmentCreateViewModel { get; set; }
        public AppointmentReadViewModel MockAppointmentReadViewModel { get; set; }
        public AppointmentUpdateViewModel MockAppointmentUpdateViewModel { get; set; }
        public IEnumerable<Appointment> MockAppointments { get; set; }
        public IEnumerable<AppointmentReadViewModel> MockAppointmentReadViewModels { get; set; }


        public readonly Appointment appointmentWithoutProcedure = new Appointment()
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

        private Appointment GenerateAppointment()
        {
            var appointment = new Appointment()
            {
                Id = 3,
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

            return appointment;
        }

        private AppointmentReadViewModel GenerateAppointmentReadViewModel()
        {
            var appointmentReadViewModel = new AppointmentReadViewModel
            {
                Id = 1,
                Date = DateTime.Now,
                Disease = "pain in leg",
                MeetHasOccureding = true,
                Procedures = new List<ProcedureReadViewModel>
                {
                     new ProcedureReadViewModel
                     {
                         Id = 3,
                         Cost = 100,
                         Name = "leg surgery",
                         Description = "leg surgery description",
                         DurationInMinutes = 35,
                         Specializations = new List<SpecializationBaseViewModel>
                         {
                            new SpecializationBaseViewModel() {Id = 17, Name = "Younger surgeon"},
                            new SpecializationBaseViewModel() {Id = 18, Name = "Master surgeon"},
                         }
                     }
                },

                Users = new List<UserReadViewModel>
                {
                    new UserReadViewModel
                    {
                        Id = 1,
                        FirstName = "Ren",
                        LastName = "Amamiya",
                        Address = "Vovchunetska",
                        BirthDate= DateTime.Now,
                        Email = "Amamiya@gmail.com",
                        Role = "doctor",
                        ProfilePicture = "none",
                        PhoneNumber = "0987654321",

                        Portfolio = new Portfolio
                        {
                            Description = "some description",
                            UserId = 1
                        },

                        Specializations = new List<Specialization>
                        {
                            new Specialization() {Id = 2, Name = "Younger surgeon"}
                        }
                    }
                },

                AnimalViewModel = new AnimalViewModel 
                {
                    Id = 1,
                    BirthDate = DateTime.Now,
                    NickName = "Tom",
                    OwnerId = 1,
                    PhotoUrl = "none"
                }
            };
            return appointmentReadViewModel;
        }

        private AppointmentCreateViewModel GenerateAppointmentCreateViewModel()
        {
            var appointmentCreateViewModel = new AppointmentCreateViewModel
            {
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3
            };
            return appointmentCreateViewModel;
        }

        private AppointmentUpdateViewModel GenerateAppointmentUpdateViewModel()
        {
            var appointmentUpdateViewModel = new AppointmentUpdateViewModel
            {
                Id = 1,
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",
                AnimalId = 3
            };
            return appointmentUpdateViewModel;
        }

        private IEnumerable<Appointment> GenerateAppointments()
        {
            var appointments = new List<Appointment>()
            {
                new Appointment()
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
                }
             };
            return appointments;
        }


        private IEnumerable<AppointmentReadViewModel> GenerateAppointmentReadViewModels()
        {
            var appointmentReadViewModel = new List<AppointmentReadViewModel>() {

                new AppointmentReadViewModel()
            {
                Id= 1,
                Date = DateTime.Now,
                MeetHasOccureding = true,
                Disease = "Broke a leg",

                AnimalViewModel = new()
                {
                    Id = 2,
                    BirthDate = DateTime.Now,
                    NickName = "Jerry",
                    OwnerId = 1,
                    PhotoUrl = "none"
                },

                Procedures = new List<ProcedureReadViewModel>
                {
                     new ProcedureReadViewModel
                     {
                         Id = 3,
                         Cost = 100,
                         Name = "leg surgery",
                         Description = "leg surgery description",
                         DurationInMinutes = 35,
                         Specializations = new List<SpecializationBaseViewModel>
                         {
                            new SpecializationBaseViewModel() {Id = 17, Name = "Younger surgeon"},
                            new SpecializationBaseViewModel() {Id = 18, Name = "Master surgeon"},
                         }
                     }
                },

                Users = new List<UserReadViewModel>
                {
                    new UserReadViewModel
                    {
                        Id = 1,
                        FirstName = "Ren",
                        LastName = "Amamiya",
                        Address = "Vovchunetska",
                        BirthDate= DateTime.Now,
                        Email = "Amamiya@gmail.com",
                        Role = "doctor",
                        ProfilePicture = "none",
                        PhoneNumber = "0987654321",

                        Portfolio = new Portfolio
                        {
                            Description = "some description",
                            UserId = 1
                        },

                        Specializations = new List<Specialization>
                        {
                            new Specialization() {Id = 2, Name = "Younger surgeon"}
                        }
                    }
                }
            }
        };

            return appointmentReadViewModel;
        }

    }

}
