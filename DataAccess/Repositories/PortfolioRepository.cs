using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ClinicContext _clinicContext;

        public PortfolioRepository(ClinicContext clinicContext) => _clinicContext = clinicContext;

        public async Task<Portfolio> GetPortfolioByIdAsync(int id)
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

        public async Task<Portfolio> UpdatePortfolioAsync(Portfolio portfolio)
        {
            _clinicContext.Portfolios.Update(portfolio);
            await _clinicContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task DeletePortfolioAsync(int id)
        {
            _clinicContext.Remove(GetPortfolioByIdAsync(id));
            await _clinicContext.SaveChangesAsync();
        }
    }
}
