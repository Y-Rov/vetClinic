using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
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
            var expected = new List<SpecializationViewModel>
            {
                new SpecializationViewModel() {Id = 0, Name = "surgeon"},
                new SpecializationViewModel() {Id = 1, Name = "worker"}
            };

            var actual = new List<Specialization>()
            {
                new Specialization {Id = 0, Name = "surgeon"},
                new Specialization {Id = 1, Name = "worker"}
            };

            _fixture.MockSpecializationService.Setup(service =>
                service.GetAllSpecializationsAsync())
            .ReturnsAsync(actual);

            _fixture.MockMapperListSpecializationViewModel.Setup(
                mapper
                    => mapper.Map(It.IsAny<IEnumerable<Specialization>>()))
                .Returns(expected);

            var specializations = 
                await _fixture.MockController.GetSpecializations();
            
            Assert.NotNull(specializations);
            Assert.Equal(expected, specializations);
        }
    }
}
