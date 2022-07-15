using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels.AnimalViewModel;
using Moq;
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

        [Fact]
        public async Task GetAllAnimals_ShouldReturnStatusCodeOk()
        {
            //Arrange
            int _ownerId = 1;

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(_animalControllerFixture.ExpectedAnimals);

            _animalControllerFixture.MockAnimalListToListMapper
                .Setup(map => map.Map(It.Is<IEnumerable<Animal>>(x => x.Equals(_animalControllerFixture.ExpectedAnimals))))
                .Returns(_animalControllerFixture.ExpectedAnimalViewModels);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController.GetAsync(_ownerId);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animalControllerFixture.ExpectedAnimalViewModels, actualResult);
        }

        [Fact]
        public async Task GetAllAnimals_ShouldReturnEmptyList()
        {
            //Arrange
            int _ownerId = 2;

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(_animalControllerFixture.ExpectedEmptyAnimals);

            _animalControllerFixture.MockAnimalListToListMapper
                .Setup(map => map.Map(It.Is<IEnumerable<Animal>>(x => x.Equals(_animalControllerFixture.ExpectedEmptyAnimals))))
                .Returns(_animalControllerFixture.ExpectedAnimalEmptyViewModels);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController.GetAsync(_ownerId);

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
            .Returns(_animalControllerFixture.ExpectedAnimal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.CreateAsync(
                        It.IsAny<Animal>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //  Act
            var actualResult =  await _animalControllerFixture.MockAnimalController
                .CreateAsync(_animalControllerFixture.ExpectedAnimalViewModel);

            //  Assert
            _animalControllerFixture.MockAnimalService.Verify();
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task CreateAnimal_ShouldReturnError()
        {
            //Arrange
            _animalControllerFixture.MockAnimalViewModelMapperUpdater
            .Setup(mapper =>
                mapper.Map(It.IsAny<AnimalViewModel>()))
            .Returns(_animalControllerFixture.ExpectedAnimal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.CreateAsync(
                        It.IsAny<Animal>()))
                .Throws<NotFoundException>();

            //Act
            var actualResult = _animalControllerFixture.MockAnimalController
                .CreateAsync(_animalControllerFixture.ExpectedAnimalViewModel);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actualResult);
        }

        [Fact]
        public async Task UpdateAnimal_ShouldReturnStatusCodeOk()
        {
            //Arrange
            _animalControllerFixture.MockAnimalViewModelMapperUpdater
                .Setup(map => map.Map(It.IsAny<AnimalViewModel>()))
                .Returns(_animalControllerFixture.ExpectedAnimal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.UpdateAsync(It.IsAny<Animal>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController
                .UpdateAsync(_animalControllerFixture.ExpectedAnimalViewModel);

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
                .Returns(_animalControllerFixture.ExpectedAnimal);

            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.UpdateAsync(It.IsAny<Animal>()))
                .Throws<NotFoundException>();

            //Act
            var actualResult = _animalControllerFixture.MockAnimalController
                .UpdateAsync(_animalControllerFixture.ExpectedAnimalViewModel);

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

        [Fact]
        public async Task GetMedCard_ShouldReturnStatusCodeOk()
        {
            //Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<AnimalParameters>()))
                .ReturnsAsync(_animalControllerFixture.ExpectedPagedList);

            _animalControllerFixture.MockPagedMedCardMapper
                .Setup(map => map.Map(It.IsAny<PagedList<Appointment>>()))
                .Returns(_animalControllerFixture.ExpectedPagedListViewModel);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController
                .GetMedCardAsync(_animalControllerFixture.pagingParameters);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animalControllerFixture.ExpectedPagedListViewModel.Entities, actualResult.Entities);
        }

        [Fact]
        public async Task GetMedCard_ShouldReturnEmptyList()
        {
            //Arrange
            _animalControllerFixture.MockAnimalService
                .Setup(ser => ser.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<AnimalParameters>()))
                .ReturnsAsync(_animalControllerFixture.ExpectedEmptyPagedList);

            _animalControllerFixture.MockPagedMedCardMapper
                .Setup(map => map.Map(It.IsAny<PagedList<Appointment>>()))
                .Returns(_animalControllerFixture.ExpectedEmptyPagedListViewModel);

            //Act
            var actualResult = await _animalControllerFixture.MockAnimalController
                .GetMedCardAsync(_animalControllerFixture.pagingParameters);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Null(actualResult.Entities);
        }
    }
}
