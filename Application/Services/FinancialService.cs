using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;

namespace Application.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly ISalaryRepository _salaryRepository;

    public FinancialService(ISalaryRepository salaryRepository)
        {
            salaryRepository = salaryRepository;
        }

        public async Task CreateSalaryAsync(Salary salary)
        {
            await _salaryRepository.CreateSalaryAsync(salary);
        }

        public async Task DeleteSalaryAsync(int id)
        {
            await _salaryRepository.DeleteSalaryAsync(id);
        }

        public async Task<Salary> GetSalaryAsync(int id)
        {
            return await _salaryRepository.GetSalaryAsync(id);
        }

        public async Task<IEnumerable<Salary>> GetSalaryAsync()
        {
            return await _salaryRepository.GetSalaryAsync();
        }

        public async Task UpdateSalaryAsync(Salary salary)
        {
            await _salaryRepository.UpdateSalaryAsync(salary);
        }
    }
}
