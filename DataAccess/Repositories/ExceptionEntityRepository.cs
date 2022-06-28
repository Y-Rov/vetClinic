using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Pagginator;
using Core.Pagginator.Parameters;
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
            var errors = PagedList<ExceptionEntity>
                .ToPagedList(searchByName, exceptionParameters.PageNumber, exceptionParameters.PageSize);
            await Task.CompletedTask;
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
            var todayErors = PagedList<ExceptionEntity>
                .ToPagedList(searchByName
                    .Where(ex => ex.DateTime.Date == DateTime.Today.Date), exceptionParameters.PageNumber, exceptionParameters.PageSize);
            await Task.CompletedTask;

            return todayErors;
        }

        public async Task<PagedList<ExceptionStats>> GetStatsAsync(ExceptionParameters exceptionParameters)
        {
            var searchByName = SearchByNameExceptions(_clinicContext.Exceptions, exceptionParameters.Name);
            var statsErrors = PagedList<ExceptionStats>
              .ToPagedList(searchByName
                    .GroupBy(ex => ex.Name)
                    .Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() }), exceptionParameters.PageNumber, exceptionParameters.PageSize);
            await Task.CompletedTask;
            return statsErrors;
        }

        public async Task<PagedList<ExceptionStats>> GetTodayStatsAsync(ExceptionParameters exceptionParameters)
        {
            var searchByName = SearchByNameExceptions(_clinicContext.Exceptions, exceptionParameters.Name);
            var todayErrorStats = PagedList<ExceptionStats>
                .ToPagedList(searchByName
                    .Where(ex => ex.DateTime.Date == DateTime.Today.Date)
                    .GroupBy(ex => ex.Name)
                    .Select(g => new ExceptionStats { Name = g.Key, Count = g.Count() }), exceptionParameters.PageNumber, exceptionParameters.PageSize);
            await Task.CompletedTask;
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
