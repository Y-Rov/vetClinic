using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;


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
            var errors = await _clinicContext.Exceptions.ToListAsync();

            return errors;
        }

        public async Task<ExceptionEntity?> GetAsync(int id)
        {
            var error = await _clinicContext.Exceptions.SingleOrDefaultAsync(ex => ex.Id == id);

            return error;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetTodayAsync()
        {
            var todayErors = await _clinicContext.Exceptions
                .Where(ex => ex.DateTime.Date == DateTime.Today.Date)
                .ToListAsync();

            return todayErors;
        }

        public async Task<IEnumerable<ExceptionStats>> GetStatsAsync()
        {
            var statsErrors = await _clinicContext.Exceptions
                .GroupBy(ex => ex.Name)
                .Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() })
                .ToListAsync();

            return statsErrors;
        }

        public async Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync()
        {
            var todayErrorStats = await _clinicContext.Exceptions.Where(ex => ex.DateTime.Date == DateTime.Today.Date)
                .GroupBy(ex => ex.Name)
                .Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() })
                .ToListAsync();

            return todayErrorStats;
        }
    }
}
