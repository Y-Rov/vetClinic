using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels.SalaryViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Accountant")]
    public class FinancialController : ControllerBase
    {
        private readonly IFinancialService _financialService;
        private readonly IUserService _userService;
        private readonly IViewModelMapper<Salary, SalaryViewModel> _readSalary;
        private readonly IViewModelMapper<SalaryViewModel, Salary> _writeSalary;
        private readonly IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>> _readSalaryList;
        private readonly IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>> _readEmployeesList;

        public FinancialController(
            IFinancialService financialService,
            IUserService userService,
            IViewModelMapper<Salary, SalaryViewModel> readSalary,
            IViewModelMapper<SalaryViewModel, Salary> writeSalary,
            IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>> readSalaryList,
            IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>> readEmployeesList)
        {
            _financialService = financialService;
            _userService = userService;
            _readSalary = readSalary;
            _writeSalary = writeSalary;
            _readSalaryList = readSalaryList;
            _readEmployeesList = readEmployeesList;
        }

        [HttpGet("/api/[controller]/")]
        public async Task<IEnumerable<SalaryViewModel>> GetAsync()
        {
            var salaries = await _financialService.GetSalaryAsync(null);
            var readSalary = _readSalaryList.Map(salaries);
            foreach(var res in readSalary)
            {
                var user = await _userService.GetUserByIdAsync(res.Id);
                res.Name = user.FirstName + " " + user.LastName;
            }
            return readSalary;
        }

        [HttpGet("/api/[controller]/{id:int:min(1)}")]
        public async Task<SalaryViewModel> GetAsync([FromRoute]int id)
        {
            var salary = await _financialService.GetSalaryByUserIdAsync(id);
            var readSalary = _readSalary.Map(salary);

            var user = await _userService.GetUserByIdAsync(readSalary.Id);
            readSalary.Name = user.FirstName + " " + user.LastName;

            return readSalary;
        }

        [HttpGet("/api/employees/")]
        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesAsync()
        {
            var employees = await _financialService.GetEmployeesWithoutSalary();
            var readEmployee = _readEmployeesList.Map(employees);
            return readEmployee;
        }

        [HttpPost]
        public async Task PostAsync(SalaryViewModel model)
        {
            var writeSalary = _writeSalary.Map(model);
            await _financialService.CreateSalaryAsync(writeSalary);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task DeleteAsync([FromRoute] int id)
        {
            await _financialService.DeleteSalaryByUserIdAsync(id);
        }

        [HttpPut]
        public async Task PutAsync(SalaryViewModel model)
        {
            var writeSalary = _writeSalary.Map(model);
            await _financialService.UpdateSalaryAsync(writeSalary);
        }

        [HttpPut("/api/financialStatements")]
        public async Task<FinancialStatement> GetFinancialStatementAsync(DateTime startDate)
        {
            var result = await _financialService.GetFinancialStatement(startDate);
            return result;
        }
    }

}
