using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IExceptionEntityService
    {
        Task<IEnumerable<ExceptionEntity>> GetAllAsync();
        Task<IEnumerable<object>> GetStatsAsync();
        Task<ExceptionEntity> GetByIdAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<object>> GetTodayStatsAsync();
    }
}
