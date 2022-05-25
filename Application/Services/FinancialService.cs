using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly ILoggerManager _logger;

        public FinancialService(ISalaryRepository salaryRepository, ILoggerManager logger)
        {
            _salaryRepository = salaryRepository;
            _logger = logger;
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            await _salaryRepository.CreateSalaryAsync(salary);
            _logger.LogInfo("Salary was created in method CreateSalaryAsync");
        }

        public async Task DeleteSalaryByUserIdAsync(int id)
        {
            Salary? salary = await _salaryRepository.GetSalaryByUserIdAsync(id);

            if (salary == null)
            {
                throw new NotFoundException($"Salary with EmployeeId {id} does not exist");
            }

            _logger.LogInfo("Salary was deleted by EmployeeId in method DeleteByUserIdAsync");
            await _salaryRepository.DeleteSalaryByUserIdAsync(id);
        }

        public async Task<Salary> GetSalaryByUserIdAsync(int id)
        {
            Salary? salary = await _salaryRepository.GetSalaryByUserIdAsync(id);
            if (salary == null)
            {
                throw new NotFoundException($"Salary with EmployeeId {id} does not exist");
            }
            _logger.LogInfo("Salary was getted by EmployeeId in method GetSalaryByUserIdAsync");
            return salary;
        }

        public async Task<IEnumerable<Salary>> GetSalaryAsync()
        {
            _logger.LogInfo("Salary was getted method GetSalaryAsync");
            return await _salaryRepository.GetSalaryAsync();
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            _logger.LogInfo("Salary was updated in method UpdateSalaryAsync");
            await _salaryRepository.UpdateSalaryAsync(salary);
        }
    }
}
