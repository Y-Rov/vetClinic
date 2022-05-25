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

        public ExceptionEntityService(IExceptionEntityRepository exceptionEntityRepository, ILoggerManager loggerManager)
        {
            _exceptionEntityRepository = exceptionEntityRepository;
            _loggerManager = loggerManager;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionEntityService method GetAsync()");

            var excpetions = _exceptionEntityRepository.GetAsync();

            _loggerManager.LogInfo("Exit ExceptionEntityService method GetAsync()");
            return await excpetions;
        }

        public async Task<ExceptionEntity> GetAsync(int id)
        {
            _loggerManager.LogInfo("Enter ExceptionEntityService method GetAsync(int id)");
            var exception = await _exceptionEntityRepository.GetAsync(id);

            if (exception == null)
            {
                _loggerManager.LogWarn("Warning in ExceptionEntityService method  GetAsync(int id)." +
                    $"There is no error with such an ID: {id}");
                throw new NotFoundException("There is no exception with such an ID");
            }

            _loggerManager.LogInfo("Exit ExceptionEntityService method GetAsync(int id)");
            return exception;
        }

        public async Task<IEnumerable<ExceptionEntity>> GetTodayAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionEntityService method  GetTodayAsync()");

            var today = await _exceptionEntityRepository.GetTodayAsync();

            _loggerManager.LogInfo("Exit ExceptionEntityService method GetTodayAsync()");
            return today;
        }

        public async Task<IEnumerable<ExceptionStats>> GetStatsAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionEntityService method  GetStatsAsync()");

            var stats = await _exceptionEntityRepository.GetStatsAsync();

            _loggerManager.LogInfo("Exit ExceptionEntityService method GetStatsAsync()");
            return stats;
        }

        public async Task<IEnumerable<ExceptionStats>> GetTodayStatsAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionEntityService method  GetTodayStatsAsync()");

            var todayStats = await _exceptionEntityRepository.GetTodayStatsAsync();

            _loggerManager.LogInfo("Exit ExceptionEntityService method  GetTodayStatsAsync()");
            return todayStats;
        }
    }
}
