using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILoggerManager _loggerManager;

        public PortfolioService(
            IPortfolioRepository portfolioRepository,
            ILoggerManager loggerManager)
        {
            _portfolioRepository = portfolioRepository;
            _loggerManager = loggerManager;
        }
        
        public async Task<Portfolio> GetPortfolioByUserIdAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetById(id);

            if (portfolio == null)
            {
                _loggerManager.LogWarn($"Portfolio with UserID = {id} doesn't exist");
                throw new NotFoundException($"Portfolio with UserID = {id} wasn't found");
            }

            _loggerManager.LogInfo($"Portfolio with UserId = {id} was found!");
            return portfolio;
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            var portfolios = await _portfolioRepository.GetAsync();

            _loggerManager.LogInfo("Getting all available portfolios");
            return portfolios;
        }

        public async Task CreatePortfolioAsync(Portfolio newPortfolio)
        {
            var possiblyExistingPortfolio = await _portfolioRepository.GetById(newPortfolio.UserId);

            if (possiblyExistingPortfolio != null)
            {
                _loggerManager.LogWarn($"User with ID = {newPortfolio.UserId} has already a portfolio");
                throw new BadRequestException($"User with ID = {newPortfolio.UserId} has already a portfolio");
            }

            await _portfolioRepository.InsertAsync(newPortfolio);
            await _portfolioRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Portfolio for user with ID = {newPortfolio.UserId} was created");
        }

        public async Task UpdatePortfolioAsync(Portfolio updatedPortfolio)
        {
            var portfolio = await GetPortfolioByUserIdAsync(updatedPortfolio.UserId);

            portfolio.Description = updatedPortfolio.Description;

            await _portfolioRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Portfolio for user with ID = {updatedPortfolio.UserId} was updated");
        }

        public async Task DeletePortfolioByUserIdAsync(int id)
        {
            var portfolio = await GetPortfolioByUserIdAsync(id);

            _portfolioRepository.Delete(portfolio);
            await _portfolioRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Portfolio for user with ID = {id} was deleted");
        }
    }
}
