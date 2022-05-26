using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;

namespace Application.Services
{
    public class ExceptionEntityService : IExceptionEntityService
    {
        private readonly IExceptionEntityRepository _exceptionEntityRepository;
        private readonly ILoggerManager _loggerManager;

        public ExceptionEntityService(
            IExceptionEntityRepository exceptionEntityRepository,
            ILoggerManager loggerManager)
        {
            _exceptionEntityRepository = exceptionEntityRepository;
            _loggerManager = loggerManager;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetAsync()
        {

            var excpetions = _exceptionEntityRepository.GetAsync();

            _loggerManager.LogInfo("Got all exceptions from ExceptionEntityService method GetAsync()");
            return await excpetions;
        }

        public async Task<ExceptionEntity> GetAsync(int id)
        {
          
            var exception = await _exceptionEntityRepository.GetAsync(id);

            if (exception is null)
            {
                _loggerManager.LogWarn("Warning in ExceptionEntityService method  GetAsync(int id)." +
                    $"There is no error with such an ID: {id}");
                throw new NotFoundException($"There is no exception with such an Id:{id}");
            }

            _loggerManager.LogInfo($"Got exception {id} from ExceptionEntityService method GetAsync()");
            return exception;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetTodayAsync()
        {
            var today = await _exceptionEntityRepository.GetTodayAsync();

            _loggerManager.LogInfo("Got all today's exceptions from ExceptionEntityService method GetAsync()");
            return today;
        }

        public async Task<IEnumerable<ExceptionStats>> GetStatsAsync()
        {
            var stats = await _exceptionEntityRepository.GetStatsAsync();

            _loggerManager.LogInfo("Got exceptions' stats from ExceptionEntityService method GetAsync()");
            return stats;
        }

        public async Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync()
        {
            var todayStats = await _exceptionEntityRepository.GetTodayStatsAsync();

            _loggerManager.LogInfo("Got today exceptions' stats from ExceptionEntityService method GetAsync()");
            return todayStats;
        }
    }
}
