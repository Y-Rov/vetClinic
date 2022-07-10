using Core.Entities;
using Core.Exceptions;
using Core.Paginator;
using Core.ViewModels;
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
            int wrongId = 20;
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
                        (It.Is<int>(id => wrongId == id)))
                .Throws<NotFoundException>();

            _fixture.MockMapperSpecializationViewModel
                .Setup(mapper =>
                    mapper.Map(It.Is<Specialization>(spec => spec == specialization)))
                .Returns(specializationViewModel);

            //  Act
            var result = _fixture.MockController.GetSpecializationById(wrongId);

            //  Assert
            Assert.NotNull(result);
            await Assert.ThrowsAsync<NotFoundException>(() => result);
        }


        [Fact]
        public async Task GetAllSpecializations_whenResultIsNotEmpty_thenStatusCodeOk()
        {
            _fixture.MockSpecializationService.Setup(service =>
                service.GetAllSpecializationsAsync(_fixture.TestParameters))
            .ReturnsAsync(_fixture.ExpectedSpecializations);

            _fixture.MockMapperPagedList.Setup(
                mapper
                    => mapper.Map(It.IsAny<PagedList<Specialization>>()))
                .Returns(_fixture.ExpectedViewModelSpecializations);

            var specializations = 
                await _fixture.MockController.GetSpecializations(_fixture.TestParameters);

            Assert.NotNull(specializations);
            Assert.NotEmpty(specializations.Entities);
        }

        [Fact]
        public async Task GetAllSpecializations_whenResultIsEmpty_thenStatusCodeOk()
        {
            PagedList<Specialization> emptyPagedList = new PagedList<Specialization>
                (new List<Specialization>(), 0, 1, 4);

            PagedReadViewModel<SpecializationViewModel> emptyViewModel = new PagedReadViewModel<SpecializationViewModel>
            {
                Entities = new List<SpecializationViewModel>(),
                CurrentPage = 1,
                TotalPages = 1,
                PageSize = 4,
                TotalCount = 0,
                HasPrevious = false,
                HasNext = false,
            };

            _fixture.MockSpecializationService.Setup(service =>
                service.GetAllSpecializationsAsync(_fixture.TestParameters))
            .ReturnsAsync(emptyPagedList);

            _fixture.MockMapperPagedList.Setup(
                mapper
                    => mapper.Map(It.IsAny<PagedList<Specialization>>()))
                .Returns(emptyViewModel);

            var specializations =
                await _fixture.MockController.GetSpecializations(_fixture.TestParameters);

            Assert.NotNull(specializations);
            Assert.Empty(specializations.Entities);
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

            Assert.Equal(result.Name, mappedSpecialization.Name);
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

            var result = _fixture.MockController.DeleteSpecialization(id);

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

            _fixture.MockSpecializationService.Setup(service =>
               service.UpdateSpecializationAsync(It.IsAny<int>(), It.IsAny<Specialization>()))
            .Returns(Task.FromResult<object?>(null))
            .Verifiable();

            var result =
                await _fixture.MockController.UpdateSpecialization(id, updatedViewModel);

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
