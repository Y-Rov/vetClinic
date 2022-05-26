using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Repositories
{
    public class ExceptionEntityRepository : IExceptionEntityRepository
    {
        private readonly ClinicContext _clinicContext;

        public ExceptionEntityRepository(ClinicContext clinicContext)
        {
            _clinicContext = clinicContext;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetAsync()
        {
            return await _clinicContext.Exceptions.ToListAsync();
        }

        public async Task<ExceptionEntity> GetAsync(int id)
        {
            return await _clinicContext.Exceptions.SingleOrDefaultAsync(ex => ex.Id == id);
        }

        public async Task<IEnumerable<ExceptionEntity>> GetTodayAsync()
        {
            return await _clinicContext.Exceptions.Where(ex => ex.DateTime.Day == DateTime.Today.Day).ToListAsync();
        }

        public async Task<IEnumerable<ExceptionStats>> GetStatsAsync()
        {
            return await _clinicContext.Exceptions.GroupBy(ex => ex.Name).Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() }).ToListAsync();
        }

        public async Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync()
        {
            return  await _clinicContext.Exceptions.Where(ex => ex.DateTime.Day == DateTime.Today.Day)
                .GroupBy(ex => ex.Name).Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() }).ToListAsync();
        }
    }
}
