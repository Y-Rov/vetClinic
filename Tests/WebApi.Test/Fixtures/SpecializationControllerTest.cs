using Core.Entities;
using Core.ViewModels.SpecializationViewModels;
using Moq;

namespace WebApi.Test.Fixtures
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
    }
}
