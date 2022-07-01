using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            MockAnimalViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<AnimalViewModel, Animal>>>();
            MockMedCardMapper = fixture.Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentViewModel>>>>();

            MockAnimalController = new AnimalController(
                MockAnimalService.Object,
                MockAnimalViewModelMapper.Object,
                MockAnimalMapper.Object,
                MockAnimalListToListMapper.Object,
                MockMedCardMapper.Object
                );
        }

        public Mock<IAnimalService> MockAnimalService { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Animal>, IEnumerable<AnimalViewModel>>> MockAnimalListToListMapper { get; }
        public Mock<IViewModelMapper<Animal, AnimalViewModel>> MockAnimalMapper { get; }
        public Mock<IViewModelMapper<AnimalViewModel, Animal>> MockAnimalViewModelMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Appointment>, IEnumerable<AppointmentViewModel>>> MockMedCardMapper { get; }
        public AnimalController MockAnimalController { get; }
    }
}
