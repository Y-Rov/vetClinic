using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IExceptionEntityRepository
    {
        Task<IEnumerable<ExceptionEntity>> GetAllAsync();
        Task<IEnumerable<object>> GetStatsAsync();
        Task<ExceptionEntity> GetByIdAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<object>> GetTodayStatsAsync();
    }
}
