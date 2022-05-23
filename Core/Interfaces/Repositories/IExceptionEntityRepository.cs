using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IExceptionEntityRepository
    {
        Task<IEnumerable<ExceptionEntity>> GetAllAsync();
        Task<IEnumerable<object>> GroupAllByName();
        Task<ExceptionEntity> GetById(int id);
        Task<IEnumerable<ExceptionEntity>> GetForThisDay();

    }
}
