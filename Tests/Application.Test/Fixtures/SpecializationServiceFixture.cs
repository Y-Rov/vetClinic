using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using Moq;

namespace Application.Test.Fixtures
{
    public class SpecializationServiceFixture
    {

        public SpecializationServiceFixture()
        {
            Expected = GenerateSpecialization();

            var fixture =
               new Fixture().Customize(new AutoMoqCustomization());

            Expected = GenerateSpecialization();
            ExpectedSpecializations = GenerateSpecializations();
            TestParameters = GenerateParameters();

            MockRepository = fixture.Freeze<Mock<ISpecializationRepository>>();
            MockUserRepository = fixture.Freeze<Mock<IUserRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

            MockService = 
                new SpecializationService(
                    MockRepository.Object,
                    MockUserRepository.Object,
                    MockLoggerManager.Object);
        }

        public SpecializationService MockService { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }
        public Mock<ISpecializationRepository> MockRepository { get; }
        public Mock<IUserRepository> MockUserRepository { get; }
        public Specialization Expected { get; set; }
        public PagedList<Specialization> ExpectedSpecializations { get; set; }
        public SpecializationParameters TestParameters { get; set; }

        private Specialization GenerateSpecialization()
        {
            var specialization = new Specialization
            {
                Id = 2,
                Name = "surgeon",
                ProcedureSpecializations = new List<ProcedureSpecialization>()
                {
                    new ProcedureSpecialization { ProcedureId = 1, SpecializationId = 2 },
                    new ProcedureSpecialization { ProcedureId = 0, SpecializationId = 2 }
                }
            };

            return specialization;
        }

        private PagedList<Specialization> GenerateSpecializations()
        {
            var specializations = new List<Specialization>()
            {
                new Specialization() { Id = 0, Name = "Surgeon"},
                new Specialization() { Id = 1, Name = "Cleaner"}
            };

            var pagedList =
                new PagedList<Specialization>(specializations,specializations.Count,1,4);

            return pagedList;
        }

        private SpecializationParameters GenerateParameters()
        {
            var parameters = new SpecializationParameters
            {
                PageSize = 4,
                PageNumber = 1
            };

            return parameters;
        }
    }
}
