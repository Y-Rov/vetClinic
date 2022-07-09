//using Application.Services;
//using AutoFixture;
//using AutoFixture.AutoMoq;
//using Core.Entities;
//using Core.Interfaces;
//using Core.Interfaces.Repositories;
//using Moq;

//namespace Application.Test.Fixtures
//{
//    public class SpecializationServiceFixture
//    {

//        public SpecializationServiceFixture()
//        {
//            Expected = GenerateSpecialization();

//            var fixture =
//               new Fixture().Customize(new AutoMoqCustomization());

//            MockRepository = fixture.Freeze<Mock<ISpecializationRepository>>();

//            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

//            MockService = 
//                new SpecializationService(MockRepository.Object,MockLoggerManager.Object);
//        }

//        public SpecializationService MockService { get; }
//        public Mock<ILoggerManager> MockLoggerManager { get; }
//        public Mock<ISpecializationRepository> MockRepository { get; }
//        public Specialization Expected { get; set; }

//        private Specialization GenerateSpecialization()
//        {
//            var specialization = new Specialization
//            {
//                Id = 2,
//                Name = "surgeon"
//            };

//            return specialization;
//        }
//    }
//}
