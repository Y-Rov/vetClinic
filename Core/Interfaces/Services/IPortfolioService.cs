using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IPortfolioService
    {
        Task<Portfolio> GetPortfolioByUserIdAsync(int id);
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task CreatePortfolioAsync(Portfolio portfolio);
        Task UpdatePortfolioAsync(Portfolio portfolio);
        Task DeletePortfolioByUserIdAsync(int id);
    }
}
