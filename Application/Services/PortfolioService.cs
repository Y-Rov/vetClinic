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

        public PortfolioService(IPortfolioRepository portfolioRepository, ILoggerManager loggerManager)
        {
            _portfolioRepository = portfolioRepository;
            _loggerManager = loggerManager;
        }
        
        public async Task<Portfolio> GetPortfolioByUserIdAsync(int id)
        {
            var portfolio = await _portfolioRepository.GetPortfolioByUserIdAsync(id);
            if (portfolio != null)
            {
                return portfolio;
            }

            _loggerManager.LogWarn($"User with ID - {id} doesn't have a portfolio!");
            throw new NotFoundException($"User with ID - {id} doesn't have a portfolio!");
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            return await _portfolioRepository.GetAllPortfoliosAsync();
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            var possiblePortfolioInTable = await _portfolioRepository.GetPortfolioByUserIdAsync(portfolio.UserId);
            if (possiblePortfolioInTable == null)
            {
                await _portfolioRepository.CreatePortfolioAsync(portfolio);
                await _portfolioRepository.SaveChangesAsync();
            }

            _loggerManager.LogWarn($"User with ID - {portfolio.UserId} has already a portfolio!");
            throw new BadRequestException($"User with ID - {portfolio.UserId} has already a portfolio!");
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            var portfolioInTable = await _portfolioRepository.GetPortfolioByUserIdAsync(portfolio.UserId);
            if (portfolioInTable != null)
            {
                await _portfolioRepository.UpdatePortfolioAsync(portfolio);
                await _portfolioRepository.SaveChangesAsync();
            }

            _loggerManager.LogWarn($"User with ID - {portfolio.UserId} doesn't have a portfolio!");
            throw new NotFoundException($"User with ID - {portfolio.UserId} doesn't have a portfolio!");
        }

        public async Task DeletePortfolioByUserIdAsync(int id)
        {
            var portfolioInTable = await _portfolioRepository.GetPortfolioByUserIdAsync(id);
            if (portfolioInTable != null)
            {
                await _portfolioRepository.DeletePortfolioByUserIdAsync(id);
                await _portfolioRepository.SaveChangesAsync();
            }

            _loggerManager.LogWarn($"User with ID - {id} doesn't have a portfolio!");
            throw new NotFoundException($"User with ID - {id} doesn't have a portfolio!");
        }
    }
}
