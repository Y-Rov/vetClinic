using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Paginator;
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

            MockAnimalService = fixture.Freeze<Mock<IAnimalService>>();
            MockAnimalListToListMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>>>();
            MockAnimalMapper = fixture.Freeze<Mock<IViewModelMapper<Animal, AnimalViewModel>>>();
            MockAnimalViewModelMapperUpdater = fixture.Freeze<Mock<IViewModelMapperUpdater<AnimalViewModel, Animal>>>();
            MockMedCardMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AnimalMedCardViewModel>>>>();
            MockPagedMedCardMapper = fixture.Freeze<Mock<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>>>>();

            MockAnimalController = new AnimalController(
                MockAnimalService.Object,
                MockAnimalViewModelMapperUpdater.Object,
                MockAnimalMapper.Object,
                MockAnimalListToListMapper.Object,
                MockMedCardMapper.Object,
                MockPagedMedCardMapper.Object
                );
        }

        public Mock<IAnimalService> MockAnimalService { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>> MockAnimalListToListMapper { get; }
        public Mock<IViewModelMapper<Animal, AnimalViewModel>> MockAnimalMapper { get; }
        public Mock<IViewModelMapperUpdater<AnimalViewModel, Animal>> MockAnimalViewModelMapperUpdater { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AnimalMedCardViewModel>>> MockMedCardMapper { get; }
        public Mock<IViewModelMapper<PagedList<Appointment>, PagedReadViewModel<AnimalMedCardViewModel>>> MockPagedMedCardMapper { get; }
        public AnimalController MockAnimalController { get; }
    }
}
