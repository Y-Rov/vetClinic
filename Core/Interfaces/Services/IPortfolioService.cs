using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IPortfolioService
    {
        Task<Portfolio> GetPortfolioByUserIdAsync(int id);
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task CreatePortfolioAsync(Portfolio newPortfolio);
        Task UpdatePortfolioAsync(Portfolio updatedPortfolio);
        Task DeletePortfolioByUserIdAsync(int id);
    }
}
