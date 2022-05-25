using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        private readonly IExceptionEntityService _exceptionEntityService;
        private readonly ILoggerManager _loggerManager;
        private readonly IViewModelMapper<IEnumerable<ExceptionEntity>,IEnumerable<ExceptionEntityReadViewModel>> _exceptionModel;
        public ExceptionController(IExceptionEntityService exceptionEntityService, ILoggerManager loggerManager,
             IViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>> exceptionModel)
        {
            _exceptionEntityService = exceptionEntityService;
            _loggerManager = loggerManager;
            _exceptionModel = exceptionModel;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExceptionEntityReadViewModel>>> GetAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionController action GetAsync()");
            var allExceptions = await _exceptionEntityService.GetAsync();

            var readDtos = _exceptionModel.Map(allExceptions);

            _loggerManager.LogInfo("Exit ExceptionController action GetAsync()");
            return Ok(readDtos);
        }

        [HttpGet("Stats")]
        public async Task<ActionResult<IEnumerable<ExceptionStats>>> GetStatsAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionController action GetStatsAsync()");
            var exceptionsStats = await _exceptionEntityService.GetStatsAsync();

            _loggerManager.LogInfo("Exit ExceptionController action GetStatsAsync()");
            return Ok(exceptionsStats);

        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ExceptionEntity>> GetAsync([FromRoute]int id)
        {
            _loggerManager.LogInfo("Enter ExceptionController action GetAsync(int id)");
            var exception = await _exceptionEntityService.GetAsync(id);

            _loggerManager.LogInfo("Exit ExceptionController action GetAsync(int id)");
            return Ok(exception);

        }

        [HttpGet("Today")]
        public async Task<ActionResult<IEnumerable<ExceptionEntityReadViewModel>>> GetTodayAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionController action GetTodayAsync()");
            var allExceptions = await _exceptionEntityService.GetTodayAsync();


            var readDtos = _exceptionModel.Map(allExceptions);

            _loggerManager.LogInfo("Exit ExceptionController action GetTodayAsync()");
            return Ok(readDtos);

        }

        [HttpGet("Stats/Today")]
        public async Task<ActionResult<IEnumerable<ExceptionStats>>> GetTodayStatsAsync()
        {
            _loggerManager.LogInfo("Enter ExceptionController action GetTodayStatsAsync()");
            var exceptionsStats = await _exceptionEntityService.GetTodayStatsAsync();

            _loggerManager.LogInfo("Exit ExceptionController action GetTodayStatsAsync()");
            return Ok(exceptionsStats);

        }
    }
}
