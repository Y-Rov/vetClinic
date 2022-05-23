using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class ExceptionEntityService : IExceptionEntityService
    {
        private readonly IExceptionEntityRepository _exceptionEntityRepository;
        public ExceptionEntityService(IExceptionEntityRepository exceptionEntityRepository)
        {
            _exceptionEntityRepository = exceptionEntityRepository;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetAllAsync()
        {
            return await _exceptionEntityRepository.GetAllAsync();
        }

        public async Task<ExceptionEntity> GetById(int id)
        {
            return await _exceptionEntityRepository.GetById(id);
        }

        public async Task<IEnumerable<ExceptionEntity>> GetForThisDay()
        {
            return await _exceptionEntityRepository.GetForThisDay();
        }

        public async Task<IEnumerable<object>> GroupAllByName()
        {
            return await _exceptionEntityRepository.GroupAllByName();
        }
    }
}
