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
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IViewModelMapper<PortfolioCreateViewModel, Portfolio> _portfolioCreateMapper;
        private readonly IViewModelMapper<Portfolio, PortfolioBaseViewModel> _portfolioReadViewModelMapper;
        private readonly IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio> _portfolioUpdateMapper;
        private readonly IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateViewModel>>
            _portfolioReadEnumerableViewModelMapper;

        public PortfolioController(
            IPortfolioService portfolioService,
            IViewModelMapper<Portfolio, PortfolioBaseViewModel> portfolioReadViewModelMapper,
            IViewModelMapper<PortfolioCreateViewModel, Portfolio> portfolioCreateMapper,
            IViewModelMapperUpdater<PortfolioBaseViewModel, Portfolio> portfolioUpdateMapper,
            IEnumerableViewModelMapper<IEnumerable<Portfolio>, IEnumerable<PortfolioCreateViewModel>> portfolioReadEnumerableViewModelMapper)
        {
            _portfolioService = portfolioService;
            _portfolioCreateMapper = portfolioCreateMapper;
            _portfolioReadViewModelMapper = portfolioReadViewModelMapper;
            _portfolioUpdateMapper = portfolioUpdateMapper;
            _portfolioReadEnumerableViewModelMapper = portfolioReadEnumerableViewModelMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PortfolioCreateViewModel>>> GetAsync()
        {
            var portfolios = await _portfolioService.GetAllPortfoliosAsync();

            var viewModels = _portfolioReadEnumerableViewModelMapper.Map(portfolios);
            return Ok(viewModels);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<PortfolioBaseViewModel>> GetAsync([FromRoute] int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByUserIdAsync(id);

            var viewModel = _portfolioReadViewModelMapper.Map(portfolio);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] PortfolioCreateViewModel portfolioCreateViewModel)
        {
            var portfolio = _portfolioCreateMapper.Map(portfolioCreateViewModel);

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