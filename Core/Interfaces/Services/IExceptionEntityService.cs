using Core.Entities;
using Core.Models;
using Core.Pagginator;

namespace Core.Interfaces.Services
{
    public interface IExceptionEntityService
    {
        Task<PagedList<ExceptionEntity>> GetAsync(PaggingParameters paggingParameters);
        Task<IEnumerable<ExceptionStats>> GetStatsAsync();
        Task<ExceptionEntity> GetAsync(int id);
        Task<IEnumerable<ExceptionEntity>> GetTodayAsync();
        Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync();
    }
}
