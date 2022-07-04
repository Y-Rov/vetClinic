using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures
{
    public class AnimalServiceFixture
    {
        public AnimalServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockAnimalRepository = fixture.Freeze<Mock<IAnimalRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();
            MockAnimalService = new AnimalService(
                MockAnimalRepository.Object,
                MockLoggerManager.Object);
        }

        public Mock<IAnimalRepository> MockAnimalRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public AnimalService MockAnimalService { get; }
    }
}
