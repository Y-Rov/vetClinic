using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.SalaryViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<SalaryViewModel>>> GetAsync()
        {
            var salaries = await _financialService.GetSalaryAsync();
            var readSalary = _readSalaryList.Map(salaries);
            foreach(var res in readSalary)
            {
                var user = await _userService.GetUserByIdAsync(res.EmployeeId);
                res.Name = user.FirstName + " " + user.LastName;
            }
            return Ok(readSalary);
        }

        [HttpGet("/api/[controller]/{id:int:min(1)}")]
        public async Task<ActionResult<SalaryViewModel>> GetAsync([FromRoute]int id)
        {
            var salary = await _financialService.GetSalaryByUserIdAsync(id);
            var readSalary = _readSalary.Map(salary);

            var user = await _userService.GetUserByIdAsync(readSalary.EmployeeId);
            readSalary.Name = user.FirstName + " " + user.LastName;

            return Ok(readSalary);
        }

        [HttpGet("/api/employees/")]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployeesAsync()
        {
            var employees = await _financialService.GetEmployeesWithoutSalary();
            var readEmployee = _readEmployeesList.Map(employees);
            return Ok(readEmployee);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]SalaryViewModel model)
        {
            var writeSalary = _writeSalary.Map(model);
            await _financialService.CreateSalaryAsync(writeSalary);
            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _financialService.DeleteSalaryByUserIdAsync(id);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync([FromBody]SalaryViewModel model)
        {
            var writeSalary = _writeSalary.Map(model);
            await _financialService.UpdateSalaryAsync(writeSalary);
            return NoContent();
        }
    }

}
