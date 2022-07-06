using Core.Entities;
using Core.Exceptions;
using Core.ViewModels;
using Core.ViewModels.AnimalViewModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class AnimalControllerTests : IClassFixture<AnimalControllerFixture>
    {
        public AnimalControllerTests(AnimalControllerFixture animalControllerFixture)
        {
            _animalControllerFixture = animalControllerFixture;
        }

        private readonly AnimalControllerFixture _animalControllerFixture;

        private readonly List<Animal> _animals = new()
        {
            new Animal()
            {
                Id = 1,
                OwnerId = 1,
                NickName = "Test1",
                BirthDate = new DateTime(2002,02,22)
            },

            new Animal()
            {
                Id = 2,
                OwnerId = 1,
                NickName = "Test1",
                BirthDate = new DateTime(2002,02,22)
            }
        };

        private readonly List<AnimalViewModel> _animalViewModels = new List<AnimalViewModel>()
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

        private readonly Animal _animal = new Animal()
        {
            Id = 3,
            OwnerId = 1,
            NickName = "Test3",
            BirthDate = new DateTime(2022, 01, 15)
        };

        private readonly AnimalViewModel _animalViewModel = new AnimalViewModel()
        {
            Id = 3,
            OwnerId = 1,
            NickName = "Test3",
            BirthDate = new DateTime(2022, 01, 15)
        };

        private readonly List<Appointment> _appointments = new()
        {
            new Appointment()
            {
                Id = 1,
                AnimalId = 1,
                Date = DateTime.Now,
                Disease = "stst",
                MeetHasOccureding = true
            },

            new Appointment()
            {
                Id = 2,
                AnimalId = 2,
                Date = DateTime.Now,
                Disease = "stst",
                MeetHasOccureding = true
            }
        };

        private readonly List<AnimalMedCardViewModel> _animalMedCardViewModel = new()
        {
            new AnimalMedCardViewModel()
            {
                Id = 1,
                AnimalId = 1,
                Date = DateTime.Now,
                Disease = "stst",
                MeetHasOccureding = true
            },

            new AnimalMedCardViewModel()
            {
                Id = 2,
                AnimalId = 3,
                Date = DateTime.Now,
                Disease = "stst",
                MeetHasOccureding = true
            }
        };

        [Fact]
        public async Task GetAnimalById_ShouldReturnStatusCodeOk()
        {
            //Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetByIdAsync(It.Is<int>(id => id == _animal.Id)))
                .ReturnsAsync(_animal);

            _animalControllerFixture.MockAnimalMapper
                .Setup(map => map.Map(It.Is<Animal>(x => x == _animal)))
                .Returns(_animalViewModel);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController.GetAsync(3);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animalViewModel, actualResult);
        }

        [Fact]
        public async Task GetAnimalById_ShouldReturnError()
        {
            //Arrange
            int incorrectId = 400;

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetByIdAsync(It.IsAny<int>()))
                .Throws<NotFoundException>();

            _animalControllerFixture.MockAnimalMapper
                .Setup(map => map.Map(It.Is<Animal>(x => x == _animal)))
                .Returns(_animalViewModel);

            //Act
            var actualResult = _animalControllerFixture.MockAnimalController.GetAsync(incorrectId);

            //Assert
            Assert.NotNull(actualResult);
            await Assert.ThrowsAsync<NotFoundException>(() => actualResult);
        }

        [Fact]
        public async Task GetAllAnimals_ShouldReturnStatusCodeOk()
        {
            //Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetAsync())
                .ReturnsAsync(_animals);

            _animalControllerFixture.MockAnimalListToListMapper
                .Setup(map => map.Map(It.Is<IEnumerable<Animal>>(x => x.Equals(_animals))))
                .Returns(_animalViewModels);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController.GetAsync();

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animalViewModels, actualResult);
        }

        [Fact]
        public async Task GetAllAnimals_ShouldReturnEmptyList()
        {
            var emptyList = new List<Animal>();
            var emptyListMap = new List<AnimalViewModel>();

            //Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetAsync())
                .ReturnsAsync(emptyList);

            _animalControllerFixture.MockAnimalListToListMapper
                .Setup(map => map.Map(It.Is<IEnumerable<Animal>>(x => x.Equals(emptyList))))
                .Returns(emptyListMap);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController.GetAsync();

            //Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult);
        }

        [Fact]
        public async Task CreateAnimal_ShouldReturnStatusCodeOk()
        {
            _animalControllerFixture.MockAnimalViewModelMapperUpdater
            .Setup(mapper =>
                mapper.Map(It.IsAny<AnimalViewModel>()))
            .Returns(_animal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.CreateAsync(
                        It.IsAny<Animal>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            var actualResult =  await _animalControllerFixture.MockAnimalController.CreateAsync(_animalViewModel);

            //  Assert
            _animalControllerFixture.MockAnimalService.Verify();
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task CreateAnimal_ShouldReturnError()
        {
            _animalControllerFixture.MockAnimalViewModelMapperUpdater
            .Setup(mapper =>
                mapper.Map(It.IsAny<AnimalViewModel>()))
            .Returns(_animal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.CreateAsync(
                        It.IsAny<Animal>()))
                .Throws<NotFoundException>();

            //  Act
            var actualResult = _animalControllerFixture.MockAnimalController.CreateAsync(_animalViewModel);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actualResult);
        }

        [Fact]
        public async Task UpdateAnimal_ShouldReturnStatusCodeOk()
        {
            //Arrange
            _animalControllerFixture.MockAnimalViewModelMapperUpdater
                .Setup(map => map.Map(It.IsAny<AnimalViewModel>()))
                .Returns(_animal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.UpdateAsync(It.IsAny<Animal>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController.UpdateAsync(_animalViewModel);

            //Assert
            _animalControllerFixture.MockAnimalService.Verify();
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task UpdateAnimal_ShouldReturnError()
        {
            //Arrange
            _animalControllerFixture.MockAnimalViewModelMapperUpdater
                .Setup(map => map.Map(It.IsAny<AnimalViewModel>()))
                .Returns(_animal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.UpdateAsync(It.IsAny<Animal>()))
                .Throws<NotFoundException>();

            //Act
            var actualResult = _animalControllerFixture.MockAnimalController.UpdateAsync(_animalViewModel);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actualResult);
        }

        [Fact]
        public async Task DeleteAnimal_ShouldReturnStatusCodeOk()
        {
            //  Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser =>
                    ser.DeleteAsync(
                        It.IsAny<int>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            await _animalControllerFixture.MockAnimalController.DeleteAsync(1);

            //  Assert
            _animalControllerFixture.MockAnimalService.Verify();
        }

        [Fact]
        public async Task DeleteAnimal_ShouldReturnError()
        {
            //  Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser =>
                    ser.DeleteAsync(
                        It.IsAny<int>()))
                .Throws<NotFoundException>();

            //  Act
            var actualResult = _animalControllerFixture.MockAnimalController.DeleteAsync(40);

            //  Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actualResult);
        }

        //[Fact]
        //public async Task GetMedCard_ShouldReturnStatusCodeOk()
        //{
        //    //Arrange
        //    _animalControllerFixture.MockAnimalService
        //        .Setup(ser => ser.GetAllAppointmentsWithAnimalIdAsync(It.Is<int>(x => x == _animal.Id)))
        //        .ReturnsAsync(_appointments);

        //    _animalControllerFixture.MockMedCardMapper
        //        .Setup(map => map.Map(It.Is<IEnumerable<Appointment>>(x => x.Equals(_appointments))))
        //        .Returns(_animalMedCardViewModel);

        //    //Act
        //    var actualResult = await _animalControllerFixture.MockAnimalController.GetMedCardAsync(_animal.Id);

        //    //Assert
        //    Assert.NotNull(actualResult);
        //    Assert.Equal(_animalMedCardViewModel, actualResult);
        //}

        //[Fact]
        //public async Task GetMedCard_ShouldReturnEmptyList()
        //{
        //    //Arrange
        //    _animalControllerFixture.MockAnimalService
        //        .Setup(ser => ser.GetAllAppointmentsWithAnimalIdAsync(It.Is<int>(x => x == _animal.Id)))
        //        .ReturnsAsync(_appointments);

        //    _animalControllerFixture.MockMedCardMapper
        //        .Setup(map => map.Map(It.Is<IEnumerable<Appointment>>(x => x.Equals(_appointments))))
        //        .Returns(_animalMedCardViewModel);

        //    //Act
        //    var actualResult = await _animalControllerFixture.MockAnimalController.GetMedCardAsync(2);

        //    //Assert
        //    Assert.NotNull(actualResult);
        //    Assert.Empty(actualResult);
        //}
    }
}
