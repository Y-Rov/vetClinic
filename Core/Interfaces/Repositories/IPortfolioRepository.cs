using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IPortfolioRepository
    {
        Task<Portfolio> GetPortfolioByIdAsync(int id);
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio);
        Task DeletePortfolioAsync(int id);
    }
}