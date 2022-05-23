using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IPortfolioService
    {
        Task<Portfolio> GetPortfolioByIdAsync(int id);
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task CreatePortfolioAsync(Portfolio portfolio);
        Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio);
        Task DeletePortfolioAsync(int id);
    }
}
