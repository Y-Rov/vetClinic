using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IExceptionEntityRepository
    {
        Task<IEnumerable<ExceptionEntity>> GetAllAsync();
        Task<IEnumerable<ExceptionStats>> GetStatsAsync();
        Task<ExceptionEntity> GetByIdAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync();
    }
}
