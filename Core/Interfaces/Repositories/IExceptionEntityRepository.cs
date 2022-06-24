using Core.Entities;
using Core.Models;
using Core.Pagginator;

namespace Core.Interfaces.Repositories
{
    public interface IExceptionEntityRepository
    {
        Task<PagedList<ExceptionEntity>> GetAsync(PaggingParameters paggingParameters);
        Task<IEnumerable<ExceptionStats>> GetStatsAsync();
        Task<ExceptionEntity?> GetAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync();
    }
}
