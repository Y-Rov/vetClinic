using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Test
{
    public class AnimalServiceTests : IClassFixture<AnimalServiceFixture>
    {
        public AnimalServiceTests(AnimalServiceFixture animalServiceFixture)
        {
            _animalServiceFixture = animalServiceFixture;
        }

        private readonly AnimalServiceFixture _animalServiceFixture;

        [Fact]
        public async Task GetAllAnimalsAsync_ShouldReturnNormalAnimalList()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetAsync(
                    It.IsAny<Expression<Func<Animal,bool>>>(),
                    It.IsAny<Func<IQueryable<Animal>, IOrderedQueryable<Animal>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_animalServiceFixture.ExpectedAnimals);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetAsync();

            //Assert
            Assert.NotEmpty(actualResult);
            Assert.Equal(_animalServiceFixture.ExpectedAnimals, actualResult);
            Assert.Equal(_animalServiceFixture.ExpectedAnimals.Count, actualResult.Count());
        }


        [Fact]
        public async Task GetAllAnimals_ShouldReturnEmptyAnimalList()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetAsync(
                    It.IsAny<Expression<Func<Animal, bool>>>(),
                    It.IsAny<Func<IQueryable<Animal>, IOrderedQueryable<Animal>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(_animalServiceFixture.ExpectedEmptyAnimals);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetAsync();

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animalServiceFixture.ExpectedEmptyAnimals, actualResult);
            Assert.Equal(_animalServiceFixture.ExpectedEmptyAnimals.Count, actualResult.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnExistingEntity()
        {
            //Arrange
            int _id = 1;

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep=>rep.GetById(
                    It.Is<int>(id => id == _id),
                    It.IsAny<string>()))
                .ReturnsAsync(_animalServiceFixture.ExpectedAnimal);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetByIdAsync(_id);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animalServiceFixture.ExpectedAnimal, actualResult);
            Assert.Equal(_animalServiceFixture.ExpectedAnimal.NickName, actualResult.NickName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException()
        {
            //Arrange
            int _id = 0;

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetById(
                    It.Is<int>(id => id == _id),
                    It.IsAny<string>()))
                .ReturnsAsync(()=>null);

            //Act
            var actualResult = _animalServiceFixture.MockAnimalService.GetByIdAsync(_id);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => actualResult);
        }

        [Fact]
        public async Task CreateAsync_ShouldSuccessfulyCreateEntity()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.InsertAsync(It.IsAny<Animal>()))
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null)).Verifiable();

            //Act
            var actualResult = _animalServiceFixture.MockAnimalService.CreateAsync(_animalServiceFixture.ExpectedAnimal);

            //Assert
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntitySuccessfuly()
        {
            //Arrange
            int _id = 1;

            var newAnimal = new Animal()
            {
                NickName = "Tolya"
            };

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetById(
                    It.Is<int>(id => id == _id),
                    It.IsAny<string>()))
                .ReturnsAsync(_animalServiceFixture.ExpectedAnimal);

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.Update(It.IsAny<Animal>()))
                .Verifiable();

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.SaveChangesAsync())
                .Returns(Task.FromResult<object?>(null))
                .Verifiable();

            //Act
            var actualResult = _animalServiceFixture.MockAnimalService.UpdateAsync(newAnimal);

            //Assert
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task GetMedCardAsync_ShouldReturnFilledAppointmentsList()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<AnimalParameters>()))
                .ReturnsAsync(_animalServiceFixture.ExpectedPagedList);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService
                .GetAllAppointmentsWithAnimalIdAsync(_animalServiceFixture.pagingParameters);

            //Assert
            Assert.NotEmpty(actualResult);
        }

        [Fact]
        public async Task GetMedCardAsync_ShouldReturnEmptyAppointmentsList()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<AnimalParameters>()))
                .ReturnsAsync(_animalServiceFixture.ExpectedEmptyPagedList);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService
                .GetAllAppointmentsWithAnimalIdAsync(_animalServiceFixture.pagingParameters);

            //Assert
            Assert.Empty(actualResult);
        }
    }
}
