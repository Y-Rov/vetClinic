using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Moq;

namespace Application.Test.Fixtures
{
    public class SpecializationServiceFixture
    {
        public SpecializationServiceFixture()
        {
            var fixture =
               new Fixture().Customize(new AutoMoqCustomization());

            Expected = GenerateSpecialization();
            ExpectedSpecializations = GenerateSpecializations();

            MockRepository = fixture.Freeze<Mock<ISpecializationRepository>>();

            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

            MockService = 
                new SpecializationService(MockRepository.Object,MockLoggerManager.Object);
        }

        public SpecializationService MockService { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public Mock<ISpecializationRepository> MockRepository { get; }
        public Specialization Expected { get; set; }
        public List<Specialization> ExpectedSpecializations { get; set; }

        private Specialization GenerateSpecialization()
        {
            var specialization = new Specialization
            {
                Id = 2,
                Name = "surgeon"
            };

            return specialization;
        }

        private List<Specialization> GenerateSpecializations()
        {
            var specializations = new List<Specialization>()
            {
                new Specialization() { Id = 0, Name = "Surgeon"},
                new Specialization() { Id = 1, Name = "Cleaner"}
            };

            return specializations;
        }
    }
}
