using Core.Entities;
using Core.Interfaces.Repositories;
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

        public async Task<IEnumerable<ExceptionEntity>> GetAllAsync()
        {
            return await _clinicContext.Exceptions.ToListAsync();
        }

        public async Task<ExceptionEntity> GetById(int id)
        {
            return await _clinicContext.Exceptions.FirstOrDefaultAsync(ex => ex.Id == id);
        }

        public async Task<IEnumerable<ExceptionEntity>> GetForThisDay()
        {
            return await _clinicContext.Exceptions.Where(ex => ex.DateTime.Day == DateTime.Today.Day).ToListAsync();
        }

        public async Task<IEnumerable<object>> GroupAllByName()
        {
            return await _clinicContext.Exceptions.GroupBy(ex => ex.Name).Select(g => new { Name = g.Key, Count = g.Count() }).ToListAsync();
        }
    }
}
