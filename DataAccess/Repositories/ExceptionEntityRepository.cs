using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
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

        public async Task<PagedList<ExceptionEntity>> GetAsync(ExceptionParameters exceptionParameters)
        {
            var searchByName = SearchByNameExceptions(_clinicContext.Exceptions, exceptionParameters.Name);
            var errors = await searchByName.ToPagedListAsync(exceptionParameters.PageNumber, exceptionParameters.PageSize);

            return errors;
        }

        public async Task<ExceptionEntity?> GetAsync(int id)
        {
            var error = await _clinicContext.Exceptions.SingleOrDefaultAsync(ex => ex.Id == id);

            return error;
        }

        public async Task<PagedList<ExceptionEntity>> GetTodayAsync(ExceptionParameters exceptionParameters)
        {
            var searchByName = SearchByNameExceptions(_clinicContext.Exceptions, exceptionParameters.Name);
            var todayErors = await searchByName
                    .Where(ex => ex.DateTime.Date == DateTime.Today.Date).
                    ToPagedListAsync(exceptionParameters.PageNumber, exceptionParameters.PageSize);

            return todayErors;
        }

        public async Task<PagedList<ExceptionStats>> GetStatsAsync(ExceptionParameters exceptionParameters)
        {
            var searchByName = SearchByNameExceptions(_clinicContext.Exceptions, exceptionParameters.Name);
            var statsErrors = await searchByName
                    .GroupBy(ex => ex.Name)
                    .Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() }).ToPagedListAsync(exceptionParameters.PageNumber, exceptionParameters.PageSize);

            return statsErrors;
        }

        public async Task<PagedList<ExceptionStats>> GetTodayStatsAsync(ExceptionParameters exceptionParameters)
        {
            var searchByName = SearchByNameExceptions(_clinicContext.Exceptions, exceptionParameters.Name);
            var todayErrorStats = await searchByName
                    .Where(ex => ex.DateTime.Date == DateTime.Today.Date)
                    .GroupBy(ex => ex.Name)
                    .Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() }).ToPagedListAsync(exceptionParameters.PageNumber, exceptionParameters.PageSize);

            return todayErrorStats;
        }
        private IQueryable<ExceptionEntity> SearchByNameExceptions(IQueryable<ExceptionEntity> exceptions, string exceptionName)
        {
            if (exceptions.Any() || !string.IsNullOrWhiteSpace(exceptionName))
            {
                return exceptions.Where(o => o.Name!.ToLower().Contains(exceptionName.Trim().ToLower()));
            }
            return exceptions;
        }
    }
}
