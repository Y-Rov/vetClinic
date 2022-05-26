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
            return await _clinicContext.Portfolios
                .AsNoTracking()
                .FirstOrDefaultAsync(portfolio => portfolio.UserId == id);
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            return await _clinicContext.Portfolios
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            await _clinicContext.Portfolios.AddAsync(portfolio);
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            _clinicContext.Portfolios.Update(portfolio);
        }

        public async Task DeletePortfolioAsync(Portfolio portfolio)
        {
            _clinicContext.Portfolios.Remove(portfolio);
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
