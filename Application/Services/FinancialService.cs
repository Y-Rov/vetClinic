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

        public FinancialService(
            ISalaryRepository salaryRepository, 
            ILoggerManager logger)
        {
            _salaryRepository = salaryRepository;
            _logger = logger;
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            var model = await _salaryRepository.GetSalaryByUserIdAsync(salary.EmployeeId);
            if(model != null)
            {
                _logger.LogWarn($"User with Id: {model.EmployeeId} already has salary");
                throw new BadRequestException($"User with Id: {model.EmployeeId} already has salary");
            }

            await _salaryRepository.CreateSalaryAsync(salary);
            await _salaryRepository.SaveChangesAsync();
            _logger.LogInfo("Salary was created in method CreateSalaryAsync");
        }

        public async Task DeleteSalaryByUserIdAsync(int id)
        {
            Salary salary = await GetSalaryByUserIdAsync(id);

            await _salaryRepository.DeleteSalaryAsync(salary);
            await _salaryRepository.SaveChangesAsync();
            _logger.LogInfo("Salary was deleted by EmployeeId in method DeleteByUserIdAsync");
        }

        public async Task<Salary> GetSalaryByUserIdAsync(int id)
        {
            Salary? salary = await _salaryRepository.GetSalaryByUserIdAsync(id);
            if (salary == null)
            {
                _logger.LogWarn($"Salary with EmployeeId = {id} does not exist");
                throw new NotFoundException($"Salary with EmployeeId {id} does not exist");
            }
            _logger.LogInfo("Salary was getted by EmployeeId in method GetSalaryByUserIdAsync");
            return salary;
        }

        public async Task<IEnumerable<Salary>> GetSalaryAsync()
        {
            var salaries = await _salaryRepository.GetSalaryAsync();
            _logger.LogInfo("Salary was getted method GetSalaryAsync");
            return salaries;
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            await GetSalaryByUserIdAsync(salary.EmployeeId); ;

            await _salaryRepository.UpdateSalaryAsync(salary);
            await _salaryRepository.SaveChangesAsync();
            _logger.LogInfo("Salary was updated in method UpdateSalaryAsync");
        }
    }
}
