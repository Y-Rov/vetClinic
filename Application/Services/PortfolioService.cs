using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository) => _portfolioRepository = portfolioRepository;
        
        public async Task<Portfolio> GetPortfolioByIdAsync(int id)
        {
            return await _portfolioRepository.GetPortfolioByIdAsync(id);
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            return await _portfolioRepository.GetAllPortfoliosAsync();
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            await _portfolioRepository.CreatePortfolioAsync(portfolio);
        }

        public async Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio)
        {
            return await _portfolioRepository.UpdatePortfolioAsync(portfolio);
        }

        public async Task DeletePortfolioAsync(int id)
        {
            await _portfolioRepository.DeletePortfolioAsync(id);
        }
    }
}
