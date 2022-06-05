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
            var portfolio = await _clinicContext.Portfolios
                .AsNoTracking()
                .FirstOrDefaultAsync(portfolio => portfolio.UserId == id);

            return portfolio;
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            var portfolios = await _clinicContext.Portfolios
                .AsNoTracking()
                .ToListAsync();

            return portfolios;
        }

        public async Task CreatePortfolioAsync(Portfolio portfolio)
        {
            await _clinicContext.Portfolios.AddAsync(portfolio);
        }

        public async Task UpdatePortfolioAsync(Portfolio portfolio)
        {
            _clinicContext.Portfolios.Update(portfolio);
            await Task.CompletedTask;
        }

        public async Task DeletePortfolioAsync(Portfolio portfolio)
        {
            _clinicContext.Portfolios.Remove(portfolio);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _clinicContext.SaveChangesAsync();
        }
    }
}
