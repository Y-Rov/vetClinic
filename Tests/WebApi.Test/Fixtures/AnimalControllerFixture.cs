using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class AnimalControllerFixture
    {
        public AnimalControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            ExpectedAnimal = GetAnimal();
            ExpectedAnimalViewModel = GetAnimalViewModel();

            ExpectedAnimals = GetAnimalList();
            ExpectedAnimalViewModels = GetAnimalViewModels();
            ExpectedAnimalEmptyViewModels = GetAnimalEmptyViewModelList();
            ExpectedEmptyAnimals = GetAnimalEmptyList();

            ExpectedPagedList = GetPagedMedCard();
            ExpectedEmptyPagedList = GetEmptyPagedList();
            ExpectedPagedListViewModel = GetPagedListViewModel();
            ExpectedEmptyPagedListViewModel = GetEmptyPagedListViewModel();

            ExpectedPdfFileModel = GetPdfFileModel();

            pagingParameters = GetAnimalParameters();

            MockAnimalService = fixture.Freeze<Mock<IAnimalService>>();
            MockAnimalListToListMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>>>();
            MockAnimalViewModelMapperUpdater = fixture.Freeze<Mock<IViewModelMapperUpdater<AnimalViewModel, Animal>>>();
            MockPagedMedCardMapper = fixture.Freeze<Mock<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>>>>();
            MockMedCardPdf = fixture.Freeze<Mock<IGenerateFullPdf<AnimalParameters>>>();

            MockAnimalController = new AnimalController(
                MockAnimalService.Object,
                MockAnimalViewModelMapperUpdater.Object,
                MockAnimalListToListMapper.Object,
                MockPagedMedCardMapper.Object,
                MockMedCardPdf.Object
                );
        }

        public Mock<IAnimalService> MockAnimalService { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>> MockAnimalListToListMapper { get; }
        public Mock<IViewModelMapperUpdater<AnimalViewModel, Animal>> MockAnimalViewModelMapperUpdater { get; }
        public Mock<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>>> MockPagedMedCardMapper { get; }
        public Mock<IGenerateFullPdf<AnimalParameters>> MockMedCardPdf { get; }
        public AnimalController MockAnimalController { get; }
        public Animal ExpectedAnimal { get; }
        public AnimalViewModel ExpectedAnimalViewModel { get; }
        public List<Animal> ExpectedAnimals { get; }
        public List<AnimalViewModel> ExpectedAnimalViewModels { get; }
        public List<AnimalViewModel> ExpectedAnimalEmptyViewModels { get; }
        public List<Animal> ExpectedEmptyAnimals { get; }
        public PagedList<Appointment> ExpectedPagedList { get; }
        public PagedList<Appointment> ExpectedEmptyPagedList { get; }
        public PagedReadViewModel<AnimalMedCardViewModel> ExpectedPagedListViewModel { get; }
        public PagedReadViewModel<AnimalMedCardViewModel> ExpectedEmptyPagedListViewModel { get; }
        public PdfFileModel ExpectedPdfFileModel { get; }

        public AnimalParameters pagingParameters;

        private AnimalParameters GetAnimalParameters()
        {
            var pagingParameters = new AnimalParameters()
            {
                animalId = 13,
                PageNumber = 1,
                PageSize = 10
            };

            return pagingParameters;
        }

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

        private AnimalViewModel GetAnimalViewModel()
        {
            var animalViewModel = new AnimalViewModel()
            {
                Id = 1,
                OwnerId = 1,
                NickName = "Tom",
                BirthDate = DateTime.Now
            };

            return animalViewModel;
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

        private List<AnimalViewModel> GetAnimalViewModels()
        {
            var animalsViewModel = new List<AnimalViewModel>()
            {
                new AnimalViewModel()
                {
                    Id = 1,
                    OwnerId = 1,
                    NickName = "Test1",
                    BirthDate = new DateTime(2002,02,22)
                },

            new AnimalViewModel()
                {
                    Id = 2,
                    OwnerId = 1,
                    NickName = "Test2",
                    BirthDate = new DateTime(2002,04,22)
                }
            };

            return animalsViewModel;
        }

        private List<Animal> GetAnimalEmptyList()
        {
            return new List<Animal>();
        }

        private List<AnimalViewModel> GetAnimalEmptyViewModelList()
        {
            return new List<AnimalViewModel>();
        }

        private PagedList<Appointment> GetPagedMedCard()
        {
            var appointments = GetAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private PagedList<Appointment> GetEmptyPagedList()
        {
            var appointments = GetEmptyAppointmentList();
            var pagedList = PagedList<Appointment>.ToPagedList(appointments.AsQueryable(), 1, 10);
            return pagedList;
        }

        private PagedReadViewModel<AnimalMedCardViewModel> GetPagedListViewModel()
        {
            var viewModel = GetAnimalMedCardViewModels();
            var pagedViewModel = new PagedReadViewModel<AnimalMedCardViewModel>
            {
                CurrentPage = 1,
                TotalCount = 1,
                PageSize = 1,
                HasNext = false,
                HasPrevious = false,
                Entities = viewModel
            };
            return pagedViewModel;
        }

        private PagedReadViewModel<AnimalMedCardViewModel> GetEmptyPagedListViewModel()
        {
            return new PagedReadViewModel<AnimalMedCardViewModel>();
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

        private List<AnimalMedCardViewModel> GetAnimalMedCardViewModels()
        {
            var viewModel = new List<AnimalMedCardViewModel>()
            {

                new AnimalMedCardViewModel()
                {
                    Id = 1,
                    AnimalId = 1,
                    Date = new DateTime(2022, 06, 20),
                    Disease = "string",
                    MeetHasOccureding = true
                },
                new AnimalMedCardViewModel()
                    {
                    Id = 2,
                    AnimalId = 2,
                    Date = new DateTime(2022, 06, 21),
                    Disease = "string",
                    MeetHasOccureding = true
                }
            };

            return viewModel;
        }

        private List<Appointment> GetEmptyAppointmentList()
        {
            return new List<Appointment>();
        }
        private PdfFileModel GetPdfFileModel()
        {

            var expectedModel = new PdfFileModel()
            {
                FileStream = new MemoryStream(),
                DefaultFileName = "pdf.pdf",
                ContentType = "application/pdf"
            };

            return expectedModel;
        }
    }
}
