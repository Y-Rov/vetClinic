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
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManager _loggerManager;

        public PortfolioService(
            IPortfolioRepository portfolioRepository,
            IUserRepository userRepository,
            ILoggerManager loggerManager)
        {
            _portfolioRepository = portfolioRepository;
            _userRepository = userRepository;
            _loggerManager = loggerManager;
        }
        
        public async Task<Portfolio> GetPortfolioByUserIdAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetPortfolioByUserIdAsync(id);

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
            var portfolios = await _portfolioRepository.GetAllPortfoliosAsync();

            _loggerManager.LogInfo("Getting all available portfolios");
            return portfolios;
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            var possiblyExistingPortfolio = await _portfolioRepository.GetPortfolioByUserIdAsync(portfolio.UserId);

            if (possiblyExistingPortfolio != null)
            {
                _loggerManager.LogWarn($"User with ID = {portfolio.UserId} has already a portfolio");
                throw new BadRequestException($"User with ID = {portfolio.UserId} has already a portfolio");
            }

            var user = await _userRepository.GetByIdAsync(portfolio.UserId);

            if (user == null)
            {
                _loggerManager.LogWarn($"User with ID = {portfolio.UserId} doesn't exist");
                throw new NotFoundException("Portfolio can't be added to non-existent user");
            }

            portfolio.User = user;
            await _portfolioRepository.CreatePortfolioAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Portfolio for user with ID = {portfolio.UserId} was created");
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            await GetPortfolioByUserIdAsync(portfolio.UserId);

            await _portfolioRepository.UpdatePortfolioAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Portfolio for user with ID = {portfolio.UserId} was updated");
        }

        public async Task DeletePortfolioByUserIdAsync(int id)
        {
            var portfolio = await GetPortfolioByUserIdAsync(id);

            await _portfolioRepository.DeletePortfolioAsync(portfolio);
            await _portfolioRepository.SaveChangesAsync();
            _loggerManager.LogInfo($"Portfolio for user with ID = {id} was deleted");
        }
    }
}
