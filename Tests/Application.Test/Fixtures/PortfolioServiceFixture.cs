using Application.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Moq;

namespace Application.Test.Fixtures
{
    public class PortfolioServiceFixture
    {
        public PortfolioServiceFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockPortfolioRepository = fixture.Freeze<Mock<IPortfolioRepository>>();
            MockLoggerManager = fixture.Freeze<Mock<ILoggerManager>>();

            MockPortfolioService = new PortfolioService(
                MockPortfolioRepository.Object,
                MockLoggerManager.Object);

            WrongUserId = 4;
            TestPortfolio = new Portfolio
            {
                UserId = 1, Description= "Ad vero soluta per, vim at wisi idque dicant. Eum ea maiorum honestatis. Illum vitae quo an, et sale vivendum sea."
            };
            TestListOfPortfolios = new List<Portfolio> { TestPortfolio };
        }

        public IPortfolioService MockPortfolioService { get; }
        public Mock<IPortfolioRepository> MockPortfolioRepository { get; }
        public Mock<ILoggerManager> MockLoggerManager { get; }

        public int WrongUserId { get; }
        public Portfolio TestPortfolio { get; }
        public IList<Portfolio> TestListOfPortfolios { get; }
    }
}
