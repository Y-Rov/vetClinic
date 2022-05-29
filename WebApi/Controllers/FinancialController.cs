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
        private readonly IViewModelMapper<Salary, SalaryViewModel> _readSalary;
        private readonly IViewModelMapper<SalaryViewModel, Salary> _writeSalary;
        private readonly IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>> _readSalaryList;

        public FinancialController(
            IFinancialService financialService,
            IViewModelMapper<Salary, SalaryViewModel> readSalary,
            IViewModelMapper<SalaryViewModel, Salary> writeSalary,
            IViewModelMapper<IEnumerable<Salary>, IEnumerable<SalaryViewModel>> readSalaryList)
        {
            _financialService = financialService;
            _readSalary = readSalary;
            _writeSalary = writeSalary;
            _readSalaryList = readSalaryList;
        }

        [HttpGet("/api/[controller]/")]
        public async Task<ActionResult<IEnumerable<SalaryViewModel>>> GetAsync()
        {
            var salaries = await _financialService.GetSalaryAsync();
            var map = _readSalaryList.Map(salaries);
            return Ok(map);
        }

        [HttpGet("/api/[controller]/{id:int:min(1)}")]
        public async Task<ActionResult<SalaryViewModel>> GetAsync([FromRoute]int id)
        {
            var salary = await _financialService.GetSalaryByUserIdAsync(id);
            var map = _readSalary.Map(salary);
            return Ok(map);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]SalaryViewModel model)
        {
            var salary = _writeSalary.Map(model);
            await _financialService.CreateSalaryAsync(salary);
            return Ok();
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
            var salary = _writeSalary.Map(model);
            await _financialService.UpdateSalaryAsync(salary);
            return Ok();
        }
    }

}
