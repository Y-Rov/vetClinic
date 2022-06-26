using Core.Entities;
using Core.Exceptions;
using Core.ViewModels.SpecializationViewModels;
using FluentAssertions;
using Moq;
using WebApi.Test.Fixtures;

namespace WebApi.Test
{
    public class SpecializationControllerTest : IClassFixture<SpecializationControllerFixture>
    {
        public SpecializationControllerTest(SpecializationControllerFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly SpecializationControllerFixture _fixture; 

        [Fact]
        public async Task GetSpecializationById_whenIdIsCorrect_thenStatusCodeOkReturned()
        {
            //  Arrange

            int id = 1;
            var specialization = new Specialization()
            {
                Id = id,
                Name = "surgeon"
            };

            var specializationViewModel = new SpecializationViewModel()
            {
                Id = id,
                Name = "surgeon"
            };

            _fixture.MockSpecializationService
                .Setup(service => 
                    service.GetSpecializationByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(specialization);

            _fixture.MockMapperSpecializationViewModel
                .Setup(mapper =>
                    mapper.Map(It.IsAny<Specialization>()))
                .Returns(specializationViewModel);

            //  Act

            var result = await _fixture.MockController.GetSpecializationById(id);

            //  Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, specializationViewModel.Name);
        }

        [Fact]
        public async Task GetSpecializationById_whenIdIsIncorrect_thenStatusCodeErrorReturned()
        {
            //  Arrange
            int id = 20;
            var specialization = new Specialization
            {
                Id = 4,
                Name = "surgeon"
            };

            var specializationViewModel = new SpecializationViewModel
            {
                Id = 4,
                Name = "surgeon"
            };

            _fixture.MockSpecializationService
                .Setup(service =>
                    service.GetSpecializationByIdAsync
                        (It.Is<int>(id => specialization.Id == id)))
                .ReturnsAsync(specialization);

            _fixture.MockMapperSpecializationViewModel
                .Setup(mapper =>
                    mapper.Map(It.Is<Specialization>(spec => spec == specialization)))
                .Returns(specializationViewModel);

            //  Act
            var result = await _fixture.MockController.GetSpecializationById(id);

            //  Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Name, specializationViewModel.Name);
        }

        [Fact]
        public async Task GetSpecializationById_whenIdIsInvalid_thenStatusCodeErrorReturned()
        {
            //  Arrange

            int id = -2;
            var specialization = new Specialization()
            {
                Id = 4,
                Name = "surgeon"
            };

            var specializationViewModel = new SpecializationViewModel()
            {
                Id = 4,
                Name = "surgeon"
            };

            _fixture.MockSpecializationService
                .Setup(service =>
                    service.GetSpecializationByIdAsync
                        (It.Is<int>(id => specialization.Id == id)))
                .ReturnsAsync(specialization);

            _fixture.MockMapperSpecializationViewModel
                .Setup(mapper =>
                    mapper.Map(It.Is<Specialization>(spec => spec == specialization)))
                .Returns(specializationViewModel);

            //  Act

            var result = await _fixture.MockController.GetSpecializationById(id);

            //  Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Name, specializationViewModel.Name);
        }

        [Fact]
        public async Task GetAllSpecializations_whenResultIsNotEmpty_thenStatusCodeOk ()
        {
            IEnumerable<SpecializationViewModel> expected = new List<SpecializationViewModel>
            {
                new SpecializationViewModel() {Id = 0, Name = "surgeon"},
                new SpecializationViewModel() {Id = 1, Name = "worker"}
            };

            IEnumerable<Specialization> returnedSpecializations = new List<Specialization>()
            {
                new Specialization {Id = 0, Name = "surgeon"},
                new Specialization {Id = 1, Name = "worker"}
            };

            _fixture.MockSpecializationService.Setup(service =>
                service.GetAllSpecializationsAsync())
            .ReturnsAsync(returnedSpecializations);

            _fixture.MockMapperListSpecializationViewModel.Setup(
                mapper
                    => mapper.Map(It.IsAny<IEnumerable<Specialization>>()))
                .Returns(new List<SpecializationViewModel>
            {
                new SpecializationViewModel() {Id = 0, Name = "surgeon"},
                new SpecializationViewModel() {Id = 1, Name = "worker"}
            });

            var specializations = 
                await _fixture.MockController.GetSpecializations();

            Assert.NotNull(specializations);
            specializations.Should().BeEquivalentTo(expected);
            //Assert.Equal(expected,specializations);
        }

        [Fact]
        public async Task GetAllSpecializations_whenResultIsEmpty_thenStatusCodeOk()
        {
            var expected = new List<SpecializationViewModel>();

            var empty = new List<Specialization>();

            var emptyViewModels = new List<SpecializationViewModel>();

            _fixture.MockSpecializationService.Setup(service =>
                service.GetAllSpecializationsAsync())
            .ReturnsAsync(empty);

            _fixture.MockMapperListSpecializationViewModel.Setup(
                mapper
                    => mapper.Map(empty))
                .Returns(emptyViewModels);

            var specializations =
                await _fixture.MockController.GetSpecializations();

            Assert.NotNull(specializations);
            Assert.Empty(specializations);
        }

        [Fact]
        public async Task AddSpecialization_whenDataIsCorrect_thenStatusCodeOk()
        {
            SpecializationViewModel specialization = new SpecializationViewModel()
            {
                Name = "cook"
            };

            Specialization mappedSpecialization = new Specialization
            {
                Name = "cook"
            };

            _fixture.MockMapperSpecialization.Setup(mapper =>
                mapper.Map(It.IsAny<SpecializationViewModel>()))
                    .Returns(mappedSpecialization);

            _fixture.MockSpecializationService.Setup(service =>
                service.AddSpecializationAsync(It.IsAny<Specialization>()))
                    .ReturnsAsync(mappedSpecialization)
                    .Verifiable();

            var result = 
                await _fixture.MockController.AddSpecialization(specialization);

            _fixture.MockSpecializationService.Verify(service =>
                service.AddSpecializationAsync(mappedSpecialization), Times.Once);

            Assert.Equal(result.Name,mappedSpecialization.Name);
        }

        [Fact]
        public async Task DeleteSpecialization_whenIdIsCorrect_ThenStatusCodeOk()
        {
            var id = 2;

            _fixture.MockSpecializationService.Setup(service =>
                service.DeleteSpecializationAsync(It.IsAny<int>()))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();

            var res = await _fixture.MockController.DeleteSpecialization(id);

            _fixture.MockSpecializationService
                .Verify(m => m.DeleteSpecializationAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteSpecialization_whenIdIsInvalid_ThenStatusCodeError()
        {
            var id = 40;

            _fixture.MockSpecializationService.Setup(service =>
                service.DeleteSpecializationAsync(id))
            .Throws<NotFoundException>();

            var result =  _fixture.MockController.DeleteSpecialization(id);

            await Assert.ThrowsAsync<NotFoundException>
                (() => result);
        }

        [Fact]
        public async Task UpdateSpecialization_whenSpecializationIsCorrect_thenStatusCodeOk()
        {
            int id = 2;
            var updatedViewModel = new SpecializationViewModel
            {
                Id = id,
                Name = "changed"
            };

            var mappedUpdated = new Specialization
            {
                Id = id,
                Name = "changed"
            };

            _fixture.MockMapperSpecialization.Setup(mapper =>
                mapper.Map(It.IsAny<SpecializationViewModel>()))
            .Returns(mappedUpdated);

            _fixture.MockSpecializationService.Setup( service =>
                service.UpdateSpecializationAsync(It.IsAny<int>(), It.IsAny<Specialization>()))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();

            var result = 
                await _fixture.MockController.UpdateSpecialization(id,updatedViewModel);

            _fixture.MockSpecializationService.Verify(service =>
                service.UpdateSpecializationAsync(id, mappedUpdated), Times.Once);
        }

        [Fact]
        public async Task UpdateSpecialization_whenSpecializationNotFound_thenReturnStatusCodeError()
        {
            int id = 90;
            var updatedViewModel = new SpecializationViewModel
            {
                Id = id,
                Name = "changed"
            };

            var mappedUpdated = new Specialization
            {
                Id = id,
                Name = "changed"
            };

            _fixture.MockMapperSpecialization.Setup(mapper =>
                mapper.Map(It.IsAny<SpecializationViewModel>()))
            .Returns(mappedUpdated);

            _fixture.MockSpecializationService.Setup(service =>
                service.UpdateSpecializationAsync(id, mappedUpdated))
            .Throws<NotFoundException>();

            var result = _fixture.MockController.UpdateSpecialization(id, updatedViewModel);

            await Assert.ThrowsAsync<NotFoundException>
                (() => result);
        }
    }
}
