using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

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

        public async Task<ExceptionEntity> GetByIdAsync(int id)
        {
            return await _exceptionEntityRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ExceptionEntity>> GetTodayAsync()
        {
            return await _exceptionEntityRepository.GetTodayAsync();
        }

        public async Task<IEnumerable<ExceptionStats>> GetStatsAsync()
        {
            return await _exceptionEntityRepository.GetStatsAsync();
        }

        public async Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync()
        {
            return await _exceptionEntityRepository.GetTodayStatsAsync();
        }
    }
}
