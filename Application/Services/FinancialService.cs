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

        private async Task GenerateUpdateSalary(Salary salary, decimal wage)
        {
            var result = new Salary();
            result.EmployeeId = salary.EmployeeId;
            result.Employee = salary.Employee;
            result.Date = DateTime.Now;
            result.Value = wage;
            await _repository.InsertAsync(result);
            await _repository.SaveChangesAsync();
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            var result = await _repository.GetById(salary.EmployeeId);

            if((result != null)&&(result.Value !=0))
            {
                _logger.LogWarn($"Employee with Id: { result.EmployeeId } has already had a salary");
                throw new BadRequestException();
            }
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
            await GenerateUpdateSalary(res, 0);
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
                if((id!=salary.EmployeeId))
                {
                    id = salary.EmployeeId;
                    if(salary.Value !=0)
                    {
                        result.Add(salary);
                    }
                }
            }
            return result;
        }

        public async Task<Salary> GetSalaryByUserIdAsync(int id)
        {
            Salary salary = await _repository.GetById(id);
            if ((salary == null)||(salary.Value == 0))
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

            await GenerateUpdateSalary(res, salary.Value);

            _logger.LogInfo($"Salary with id: {res.EmployeeId} updated");
        }
    }
    
}
