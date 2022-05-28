using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IExceptionEntityRepository
    {
        Task<IEnumerable<ExceptionEntity>> GetAsync();
        Task<IEnumerable<ExceptionStats>> GetStatsAsync();
        Task<ExceptionEntity?> GetAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync();
    }
}
