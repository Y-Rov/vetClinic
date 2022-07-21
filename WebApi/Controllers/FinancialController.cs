using Core.Entities;
using Core.Interfaces.Services;
using Core.Interfaces.Services.PDF_Service;
using Core.Models.Finance;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
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
        private readonly IViewModelMapper<PagedList<Salary>, PagedReadViewModel<SalaryViewModel>> _readSalaryList;
        private readonly IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>> _readEmployeesList;
        private readonly IViewModelMapper<PagedList<FinancialStatement>, PagedReadViewModel<FinancialStatementForMonthViewModel>> _finStatViewModel;
        private readonly IGenerateFullPDF<FinancialStatementParameters> _generatePDF;

        public FinancialController(
            IFinancialService financialService,
            IUserService userService,
            IViewModelMapper<Salary, SalaryViewModel> readSalary,
            IViewModelMapper<SalaryViewModel, Salary> writeSalary,
            IViewModelMapper<PagedList<Salary>, PagedReadViewModel<SalaryViewModel>> readSalaryList,
            IViewModelMapper<IEnumerable<User>, IEnumerable<EmployeeViewModel>> readEmployeesList,
            IViewModelMapper<PagedList<FinancialStatement>, PagedReadViewModel<FinancialStatementForMonthViewModel>> finStatViewModel,
            IGenerateFullPDF<FinancialStatementParameters> generatePDF
            )
        {
            _financialService = financialService;
            _userService = userService;
            _readSalary = readSalary;
            _writeSalary = writeSalary;
            _readSalaryList = readSalaryList;
            _readEmployeesList = readEmployeesList;
            _finStatViewModel = finStatViewModel;
            _generatePDF = generatePDF;
        }

        [HttpGet("generatePDF")]
        public async Task<FileStreamResult> GeneratePDF([FromQuery] FinancialStatementParameters animalParameters)
        {
            var pdfFileParams = await _generatePDF.GeneratePDF(animalParameters);
            return File(pdfFileParams.FileStream, pdfFileParams.ContentType, pdfFileParams.DefaultFileName);
        }

        [HttpGet]
        public async Task<PagedReadViewModel<SalaryViewModel>> GetAsync([FromQuery] SalaryParametrs parametrs)
        {
            var salaries = await _financialService.GetSalaryAsync(parametrs);
            var readSalary = _readSalaryList.Map(salaries);
            foreach(var res in readSalary.Entities)
            {
                var user = await _userService.GetUserByIdAsync(res.Id);
                res.Name = user.FirstName + " " + user.LastName;
            }
            return readSalary;
        }

        [HttpGet("{id:int:min(1)}")]
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

        [HttpGet("/api/financialStatements")]
        public async Task<PagedReadViewModel<FinancialStatementForMonthViewModel>> GetFinancialStatementAsync(
            [FromQuery] FinancialStatementParameters parameters)
        {
            parameters.Date = new DatePeriod()
            {
                StartDate = parameters.Date.StartDate.ToLocalTime(),
                EndDate = parameters.Date.EndDate.ToLocalTime()
            };
            var result = await _financialService.GetFinancialStatement(parameters);
            var finViewModel = _finStatViewModel.Map(result);
            return finViewModel;
        }
    }

}
