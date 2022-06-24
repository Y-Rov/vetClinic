using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Pagginator;
using Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/exceptions")]
    [ApiController]
   // [Authorize(Roles = "Admin")]
    public class ExceptionController : ControllerBase
    {
        private readonly IExceptionEntityService _exceptionEntityService;
        private readonly IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>> _exceptionModel;

        public ExceptionController(
            IExceptionEntityService exceptionEntityService,
            IEnumerableViewModelMapper<IEnumerable<ExceptionEntity>, IEnumerable<ExceptionEntityReadViewModel>> exceptionModel)
        {
            _exceptionEntityService = exceptionEntityService;
            _exceptionModel = exceptionModel;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ExceptionEntityReadViewModel>>> GetAsync([FromQuery] PaggingParameters paggingParameters)
        {
            var allExceptions = await _exceptionEntityService.GetAsync(paggingParameters);

            var readDtos = _exceptionModel.Map(allExceptions);

            return Ok(readDtos);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<IEnumerable<ExceptionStats>>> GetStatsAsync()
        {
            var exceptionsStats = await _exceptionEntityService.GetStatsAsync();

            return Ok(exceptionsStats);

        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ExceptionEntity>> GetAsync([FromRoute] int id)
        {
            var exception = await _exceptionEntityService.GetAsync(id);

            return Ok(exception);

        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<ExceptionEntityReadViewModel>>> GetTodayAsync()
        {
            var allExceptions = await _exceptionEntityService.GetTodayAsync();

            var readDtos = _exceptionModel.Map(allExceptions);

            return Ok(readDtos);

        }

        [HttpGet("stats/today")]
        public async Task<ActionResult<IEnumerable<ExceptionStats>>> GetTodayStatsAsync()
        {
            var exceptionsStats = await _exceptionEntityService.GetTodayStatsAsync();

            return Ok(exceptionsStats);

        }
    }
}
