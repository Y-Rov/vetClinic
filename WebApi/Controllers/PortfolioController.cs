using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.PortfolioViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/portfolios")]
    [ApiController]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IViewModelMapper<PortfolioCreateReadViewModel, Portfolio> _portfolioCreateMapper;
        private readonly IViewModelMapper<Portfolio, PortfolioBaseViewModel> _portfolioReadViewModelMapper;
        private readonly IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio> _portfolioUpdateMapper;
        private readonly IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateReadViewModel>>
            _portfolioReadEnumerableViewModelMapper;

        public PortfolioController(
            IPortfolioService portfolioService,
            IViewModelMapper<PortfolioCreateReadViewModel, Portfolio> portfolioCreateMapper,
            IViewModelMapper<Portfolio, PortfolioBaseViewModel> portfolioReadViewModelMapper,
            IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio> portfolioUpdateMapper,
            IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateReadViewModel>> portfolioReadEnumerableViewModelMapper)
        {
            _portfolioService = portfolioService;
            _portfolioCreateMapper = portfolioCreateMapper;
            _portfolioReadViewModelMapper = portfolioReadViewModelMapper;
            _portfolioUpdateMapper = portfolioUpdateMapper;
            _portfolioReadEnumerableViewModelMapper = portfolioReadEnumerableViewModelMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<PortfolioCreateReadViewModel>> GetAsync()
        {
            var portfolios = await _portfolioService.GetAllPortfoliosAsync();

            var mappedPortfolios = _portfolioReadEnumerableViewModelMapper.Map(portfolios);
            return mappedPortfolios;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<PortfolioBaseViewModel> GetAsync([FromRoute] int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByUserIdAsync(id);

            var mappedPortfolio = _portfolioReadViewModelMapper.Map(portfolio);
            return mappedPortfolio;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] PortfolioCreateReadViewModel portfolioCreateReadViewModel)
        {
            var portfolio = _portfolioCreateMapper.Map(portfolioCreateReadViewModel);

            await _portfolioService.CreatePortfolioAsync(portfolio);
            return NoContent();
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] PortfolioBaseViewModel portfolioBaseViewModel)
        {
            var portfolio = await _portfolioService.GetPortfolioByUserIdAsync(id);
            _portfolioUpdateMapper.Map(portfolioBaseViewModel, portfolio);

            await _portfolioService.UpdatePortfolioAsync(portfolio);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _portfolioService.DeletePortfolioByUserIdAsync(id);
            return NoContent();
        }
    }
}