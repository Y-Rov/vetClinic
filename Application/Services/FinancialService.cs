using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly ISalaryRepository _repository;
        private readonly ILoggerManager _logger;

        public FinancialService(
            ISalaryRepository repository,
            ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            await _repository.InsertAsync(salary);
            await _repository.SaveChangesAsync();
            _logger.LogInfo("Salary added");
        }

        public async Task DeleteSalaryByUserIdAsync(int id)
        {
            var res = await GetSalaryByUserIdAsync(id);
            if(res.Value == 0)
            {
                _logger.LogWarn($"There is no salary with id: {id}");
                throw new NotFoundException($"Salary with id: {id} not found");
            }
            var salary = new Salary();
            salary.EmployeeId = id;
            salary.Employee = res.Employee;
            salary.Value = 0;
            salary.Date = DateTime.Now;
            await CreateSalaryAsync(salary);
            _logger.LogInfo($"Salary with id: {id} deleted");
        }

        public async Task<IEnumerable<Salary>> GetSalaryAsync()
        {
            _logger.LogInfo($"salaries were recieved");
            var allSalary = await _repository.GetAsync(filter: null, x => x.OrderBy(y => y.EmployeeId).ThenByDescending(y => y.Date));
            var result = new List<Salary>();
            int? id = null;

            foreach (var salary in allSalary)
            {
                if(id!=salary.EmployeeId)
                {
                    id = salary.EmployeeId;
                    result.Add(salary);
                }
            }
            return result;
        }

        public async Task<Salary> GetSalaryByUserIdAsync(int id)
        {
            Salary salary = await _repository.GetById(id);
            if (salary is null)
            {
                _logger.LogWarn($"Salary with id: {id} not found");
                throw new NotFoundException($"Salary with id: {id} not found");
            }
            _logger.LogInfo($"Salary with id: {id} found");
            return salary;
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            var res = await GetSalaryByUserIdAsync(salary.EmployeeId);
            var _salary = new Salary();
            _salary.EmployeeId = res.EmployeeId;
            _salary.Employee = res.Employee;
            _salary.Value = salary.Value;
            _salary.Date = DateTime.Now;
            await CreateSalaryAsync(_salary);
            _logger.LogInfo($"Salary with id: {res.EmployeeId} updated");
        }
    }
    
}
