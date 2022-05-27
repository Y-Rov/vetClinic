using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.PortfolioViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/portfolios")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IViewModelMapper<Portfolio, PortfolioViewModel> _portfolioViewModelMapper;
        private readonly IViewModelMapper<PortfolioViewModel, Portfolio> _portfolioMapper;

        public PortfolioController(
            IPortfolioService portfolioService,
            IViewModelMapper<Portfolio, PortfolioViewModel> portfolioViewModelMapper,
            IViewModelMapper<PortfolioViewModel, Portfolio> portfolioMapper)
        {
            _portfolioService = portfolioService;
            _portfolioViewModelMapper = portfolioViewModelMapper;
            _portfolioMapper = portfolioMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioViewModel>>> GetAsync()
        {
            var portfolios = await _portfolioService.GetAllPortfoliosAsync();

            var viewModels = portfolios.Select(p => _portfolioViewModelMapper.Map(p));
            return Ok(viewModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<PortfolioViewModel>> GetAsync([FromRoute] int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByUserIdAsync(id);

            var viewModel = _portfolioViewModelMapper.Map(portfolio);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(PortfolioViewModel portfolioViewModel)
        {
            var portfolio = _portfolioMapper.Map(portfolioViewModel);

            await _portfolioService.CreatePortfolioAsync(portfolio);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(PortfolioViewModel portfolioViewModel)
        {
            var portfolio = _portfolioMapper.Map(portfolioViewModel);

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