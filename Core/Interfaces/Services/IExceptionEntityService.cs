using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IExceptionEntityService
    {
        Task<IEnumerable<ExceptionEntity>> GetAllAsync();
        Task<IEnumerable<object>> GroupAllByName();
        Task<ExceptionEntity> GetById(int id);
        Task<IEnumerable<ExceptionEntity>> GetForThisDay();
    }
}
