using Core.Entities;
using Core.Models;
using Core.Pagginator;
using Core.Pagginator.Parameters;

namespace Core.Interfaces.Repositories
{
    public interface IExceptionEntityRepository
    {
        Task<PagedList<ExceptionEntity>> GetAsync(ExceptionParameters exceptionParameters);
        Task<PagedList<ExceptionStats>> GetStatsAsync(ExceptionParameters exceptionParameters);
        Task<ExceptionEntity?> GetAsync(int id);
        Task<PagedList<ExceptionEntity>> GetTodayAsync(ExceptionParameters exceptionParameters);
        Task<PagedList<ExceptionStats>> GetTodayStatsAsync(ExceptionParameters exceptionParameters);
    }
}
