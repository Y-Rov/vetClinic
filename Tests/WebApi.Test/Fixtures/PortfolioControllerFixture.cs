using AutoFixture;
using AutoFixture.AutoMoq;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.PortfolioViewModels;
using Moq;
using WebApi.AutoMapper.Interface;
using WebApi.Controllers;

namespace WebApi.Test.Fixtures
{
    public class PortfolioControllerFixture
    {
        public PortfolioControllerFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            MockPortfolioService = fixture.Freeze<Mock<IPortfolioService>>();
            MockPortfolioCreateMapper = fixture.Freeze<Mock<IViewModelMapper<PortfolioCreateReadViewModel, Portfolio>>>();
            MockPortfolioReadViewModelMapper = fixture.Freeze<Mock<IViewModelMapper<Portfolio, PortfolioBaseViewModel>>>();
            MockPortfolioUpdateMapper = fixture.Freeze<Mock<IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio>>>();
            MockPortfolioReadEnumerableViewModelMapper = fixture
                .Freeze<Mock<IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateReadViewModel>>>>();

            MockPortfolioController = new PortfolioController(
                MockPortfolioService.Object,
                MockPortfolioCreateMapper.Object,
                MockPortfolioReadViewModelMapper.Object,
                MockPortfolioUpdateMapper.Object,
                MockPortfolioReadEnumerableViewModelMapper.Object
            );

            UserId = 1;
            ReadyPortfolio = new Portfolio
            {
                UserId = UserId, Description = "Lorem ipsum dolor sit amet, duo modus equidem te, nulla nostrud ne pro. Ea quo tota dicunt. Mutat quodsi tamquam vis ex."
            };
            ReadyPortfolioBaseViewModel = new PortfolioBaseViewModel
            {
                Description = "Lorem ipsum dolor sit amet, duo modus equidem te, nulla nostrud ne pro. Ea quo tota dicunt. Mutat quodsi tamquam vis ex."
            };
            ReadyPortfolioCreateReadViewModel = new PortfolioCreateReadViewModel
            {
                Id = UserId, Description = "Lorem ipsum dolor sit amet, duo modus equidem te, nulla nostrud ne pro. Ea quo tota dicunt. Mutat quodsi tamquam vis ex."
            };
            Portfolios = new List<Portfolio> { ReadyPortfolio };
            PortfolioCreateReadViewModels = new List<PortfolioCreateReadViewModel> { ReadyPortfolioCreateReadViewModel };
        }

        public PortfolioController MockPortfolioController { get; }
        public Mock<IPortfolioService> MockPortfolioService { get; }
        public Mock<IViewModelMapper<PortfolioCreateReadViewModel, Portfolio>> MockPortfolioCreateMapper { get; }
        public Mock<IViewModelMapper<Portfolio, PortfolioBaseViewModel>> MockPortfolioReadViewModelMapper { get; }
        public Mock<IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio>> MockPortfolioUpdateMapper { get; }
        public Mock<IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateReadViewModel>>> MockPortfolioReadEnumerableViewModelMapper
        {
            get;
        }

        public int UserId { get; }
        public Portfolio ReadyPortfolio { get; }
        public PortfolioBaseViewModel ReadyPortfolioBaseViewModel { get; }
        public PortfolioCreateReadViewModel ReadyPortfolioCreateReadViewModel { get; }
        public IEnumerable<Portfolio> Portfolios { get; }
        public IEnumerable<PortfolioCreateReadViewModel> PortfolioCreateReadViewModels { get; }
    }
}
