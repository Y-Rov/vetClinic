using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;

namespace Application.Test.Fixtures
{
    public class AnimalServiceFixture
    {
        public AnimalServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            ExpectedAnimal = GetAnimal();
            ExpectedAnimals = GetAnimalList();
            ExpectedEmptyAnimals = GetAnimalEmptyList();
            ExpectedPagedList = GetPagedMedCard();
            ExpectedEmptyPagedList = GetEmptyPagedMedCard();

            pagingParameters = new()
            {
                PageNumber = 1,
                PageSize = 10
            };

            MockAnimalRepository = fixture.Freeze<Mock<IAnimalRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
            MockAnimalPhotoService = fixture.Freeze<Mock<IAnimalPhotoService>>();

            MockAnimalService = new AnimalService(
                MockAnimalRepository.Object,
                MockLoggerManager.Object,
                MockAnimalPhotoService.Object);


        }

        public Mock<IAnimalRepository> MockAnimalRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public Mock<IAnimalPhotoService> MockAnimalPhotoService { get; }
        public AnimalService MockAnimalService { get; }

        public Animal ExpectedAnimal { get; }
        public List<Animal> ExpectedAnimals { get; }
        public List<Animal> ExpectedEmptyAnimals { get; }
        public PagedList<Appointment> ExpectedPagedList { get; }
        public PagedList<Appointment> ExpectedEmptyPagedList { get; }

        public AnimalParameters pagingParameters;

        private Animal GetAnimal()
        {
            var animal = new Animal()
            {
                Id = 1,
                OwnerId = 1,
                NickName = "Tom",
                BirthDate = DateTime.Now
            };

            return animal;
        }

        private List<Animal> GetAnimalList()
        {
            var animals = new List<Animal>()
            {
                new Animal()
                {
                    Id = 2,
                    OwnerId = 1,
                    NickName = "Bom",
                    BirthDate = DateTime.Now
                },
                new Animal()
                {
                    Id = 3,
                    OwnerId = 1,
                    NickName = "Mob",
                    BirthDate = DateTime.Now
                }
            };
            return animals;
        }

        private List<Animal> GetAnimalEmptyList()
        {
            return new List<Animal>();
        }

        private PagedList<Appointment> GetPagedMedCard()
        {
            var appointments = GetAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private PagedList<Appointment> GetEmptyPagedMedCard()
        {
            var appointments = GetEmptyAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private List<Appointment> GetAppointmentList()
        {
            var appointments = new List<Appointment>()
            {

                new Appointment()
                {
                    Id = 1,
                    AnimalId = 1,
                    Date = new DateTime(2022, 06, 20),
                    Disease = "string",
                    MeetHasOccureding = true
                },
                new Appointment()
                {
                    Id = 2,
                    AnimalId = 2,
                    Date = new DateTime(2022, 06, 21),
                    Disease = "string",
                    MeetHasOccureding = true
                }
            };

            return appointments;
        }

        private List<Appointment> GetEmptyAppointmentList()
        {
            return new List<Appointment>();
        }
    }
}
