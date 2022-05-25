using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILoggerManager _loggerManager;

        public PortfolioService(IPortfolioRepository portfolioRepository, ILoggerManager loggerManager)
        {
            _portfolioRepository = portfolioRepository;
            _loggerManager = loggerManager;
        }
        
        public async Task<Portfolio> GetPortfolioByUserIdAsync(int id)
        {
            _loggerManager.LogInfo("Inside GetPortfolioByUserIdAsync");
            var portfolio = await _portfolioRepository.GetPortfolioByUserIdAsync(id);
            if (portfolio != null)
            {
                _loggerManager.LogInfo("Return from GetPortfolioByUserIdAsync");
                return portfolio;
            }

            _loggerManager.LogError($"User with ID - {id} doesn't have a portfolio!");
            throw new NullReferenceException();
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            _loggerManager.LogInfo("Inside GetAllPortfoliosAsync");
            return await _portfolioRepository.GetAllPortfoliosAsync();
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            _loggerManager.LogInfo("Inside CreatePortfolioAsync");
            var portfolioInTable = await _portfolioRepository.GetPortfolioByUserIdAsync(portfolio.UserId);
            if (portfolioInTable == null)
            {
                _loggerManager.LogInfo("Creating user's portfolio...");
                await _portfolioRepository.CreatePortfolioAsync(portfolio);
            }

            _loggerManager.LogError($"User with ID - {portfolio.UserId} has already a portfolio!");
            throw new ArgumentException();
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            _loggerManager.LogInfo("Inside UpdatePortfolioAsync");
            var portfolioInTable = await _portfolioRepository.GetPortfolioByUserIdAsync(portfolio.UserId);
            if (portfolioInTable != null)
            {
                _loggerManager.LogInfo("Updating user's portfolio...");
                await _portfolioRepository.UpdatePortfolioAsync(portfolio);
            }

            _loggerManager.LogError($"User with ID - {portfolio.UserId} doesn't have a portfolio!");
            throw new NullReferenceException();
        }

        public async Task DeletePortfolioByUserIdAsync(int id)
        {
            _loggerManager.LogInfo("Inside DeletePortfolioByUserIdAsync");
            var portfolioInTable = await _portfolioRepository.GetPortfolioByUserIdAsync(id);
            if (portfolioInTable != null)
            {
                _loggerManager.LogInfo("Deleting user's portfolio...");
                await _portfolioRepository.DeletePortfolioByUserIdAsync(id);
            }

            _loggerManager.LogError($"User with ID - {id} doesn't have a portfolio!");
            throw new NullReferenceException();
        }
    }
}
