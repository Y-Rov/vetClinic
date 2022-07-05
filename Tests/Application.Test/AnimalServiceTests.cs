using System.Linq.Expressions;
using Application.Test.Fixtures;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
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

        private readonly Animal _animal = new()
        {
            Id = 141,
            NickName = "Bob Ross",
            OwnerId = 1,
            BirthDate = new DateTime(1999, 05, 11)
        };

        private readonly List<Appointment> _appointments = new()
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
            },
        };

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
                .ReturnsAsync(_animals);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetAsync();

            //Assert
            Assert.NotEmpty(actualResult);
            Assert.Equal(_animals, actualResult);
        }


        [Fact]
        public async Task GetAllAnimals_ShouldReturnEmptyAnimalList()
        {
            //Arrange
            var empyListOfAnimals = new List<Animal>();

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetAsync(
                    It.IsAny<Expression<Func<Animal, bool>>>(),
                    It.IsAny<Func<IQueryable<Animal>, IOrderedQueryable<Animal>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(empyListOfAnimals);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetAsync();

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(empyListOfAnimals, actualResult);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnExistingEntity()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep=>rep.GetById(
                    It.Is<int>(id => id == _animal.Id),
                    It.IsAny<string>()))
                .ReturnsAsync(_animal);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetByIdAsync(_animal.Id);

            //Assert
            Assert.NotNull(actualResult);
            Assert.Equal(_animal, actualResult);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException()
        {
            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetById(
                    It.Is<int>(id => id == _animal.Id),
                    It.IsAny<string>()))
                .ReturnsAsync(()=>null);

            //Act
            var actualResult = _animalServiceFixture.MockAnimalService.GetByIdAsync(1111);

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
            var actualResult = _animalServiceFixture.MockAnimalService.CreateAsync(_animal);

            //Assert
            Assert.NotNull(actualResult);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntitySuccessfuly()
        {
            //Arrange
            var newAnimal = new Animal()
            {
                NickName = "Tolya"
            };

            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetById(
                    It.Is<int>(id => id == _animal.Id),
                    It.IsAny<string>()))
                .ReturnsAsync(_animal);

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
                .Setup(rep => rep.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<int>()))
                .ReturnsAsync(_appointments);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetAllAppointmentsWithAnimalIdAsync(1);

            //Assert
            Assert.NotEmpty(actualResult);
        }

        [Fact]
        public async Task GetMedCardAsync_ShouldReturnEmptyAppointmentsList()
        {
            var emptyListOfAppointments = new List<Appointment>();

            //Arrange
            _animalServiceFixture.MockAnimalRepository
                .Setup(rep => rep.GetAllAppointmentsWithAnimalIdAsync(It.IsAny<int>()))
                .ReturnsAsync(emptyListOfAppointments);

            //Act
            var actualResult = await _animalServiceFixture.MockAnimalService.GetAllAppointmentsWithAnimalIdAsync(2);

            //Assert
            Assert.Empty(actualResult);
        }
    }
}
