using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ClinicContext _clinicContext;

        public PortfolioRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }
        
        public async Task<Portfolio?> GetPortfolioByUserIdAsync(int id)
        {
            return await _clinicContext.Portfolios.FirstOrDefaultAsync(portfolio => portfolio.UserId == id);
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            return await _clinicContext.Portfolios.ToListAsync();
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            await _clinicContext.Portfolios.AddAsync(portfolio);
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            _clinicContext.Portfolios.Update(portfolio);
            await _clinicContext.SaveChangesAsync();
        }

        public async Task DeletePortfolioByUserIdAsync(int id)
        {
            _clinicContext.Remove(GetPortfolioByUserIdAsync(id));
            await _clinicContext.SaveChangesAsync();
        }
    }
}
