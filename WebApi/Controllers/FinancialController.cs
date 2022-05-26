using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.SalaryViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;
using WebApi.Validators;
using Core.Exceptions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly IFinancialService _financialService;
        private readonly IViewModelMapper<Salary, SalaryViewModel> _mapperMtoVM;
        private readonly IViewModelMapper<SalaryViewModel, Salary> _mapperVMtoM;
        private readonly SalaryValidator _salaryValidator;

        public FinancialController(
            IFinancialService financialService,
            IViewModelMapper<Salary, SalaryViewModel> mapperMtoVM,
            IViewModelMapper<SalaryViewModel, Salary> mapperVMtoM,
            SalaryValidator salaryValidator)
        {
            _financialService = financialService;
            _mapperMtoVM = mapperMtoVM;
            _mapperVMtoM = mapperVMtoM;
            _salaryValidator = salaryValidator;
        }

        [HttpGet("/api/finances/")]
        public async Task<ActionResult<IEnumerable<SalaryViewModel>>> GetAsync()
        {
            var salaries = await _financialService.GetSalaryAsync();
            var map = new List<SalaryViewModel>();
            foreach (var salary in salaries)
            {
                map.Add(_mapperMtoVM.Map(salary));
            }
            return Ok(map);
        }

        [HttpGet("/api/finances/{id:int:min(1)}")]
        public async Task<ActionResult<SalaryViewModel>> GetAsync(int id)
        {
            var salary = await _financialService.GetSalaryByUserIdAsync(id);
            var map = _mapperMtoVM.Map(salary);
            return Ok(map);
        }

        [HttpPost("api/finances")]
        public async Task<ActionResult> PostAsync(SalaryViewModel model)
        {
            var validResult = await _salaryValidator.ValidateAsync(model);
            if(!validResult.IsValid)
            {
                throw new BadRequestException(validResult.Errors.ToString());
            }
            var salary = _mapperVMtoM.Map(model);
            await _financialService.CreateSalaryAsync(salary);
            return Ok();
        }

        [HttpDelete("api/finances/{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _financialService.DeleteSalaryByUserIdAsync(id);
            return NoContent();
        }

        [HttpPut("api/finances")]
        public async Task<ActionResult> PutAsync(SalaryViewModel model)
        {
            var validResult = await _salaryValidator.ValidateAsync(model);
            if(!validResult.IsValid)
            {
                throw new BadRequestException(validResult.Errors.ToString());
            }
            var salary = _mapperVMtoM.Map(model);
            await _financialService.UpdateSalaryAsync(salary);
            return Ok();
        }
    }

}
