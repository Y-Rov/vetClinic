using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IPortfolioRepository
    {
        Task<Portfolio?> GetPortfolioByUserIdAsync(int id);
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task CreatePortfolioAsync(Portfolio portfolio);
        Task UpdatePortfolioAsync(Portfolio portfolio);
        Task DeletePortfolioByUserIdAsync(int id);
    }
}