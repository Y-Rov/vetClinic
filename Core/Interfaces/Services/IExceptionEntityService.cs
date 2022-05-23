using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface IExceptionEntityService
    {
        Task<IEnumerable<ExceptionEntity>> GetAllAsync();
        Task<IEnumerable<ExceptionStats>> GetStatsAsync();
        Task<ExceptionEntity> GetByIdAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync();
    }
}
