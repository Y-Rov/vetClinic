using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController : ControllerBase
    {
        private readonly IExceptionEntityService _exceptionEntityService;

        public ExceptionController(IExceptionEntityService exceptionEntityService)
        {
            _exceptionEntityService = exceptionEntityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExceptionEntity>>> GetAllAsync()
        {
            var allExceptions = await _exceptionEntityService.GetAllAsync();

            if (allExceptions == null)
                throw new NotFoundException();

            return Ok(allExceptions);
        }

        [HttpGet("Stats")]
        public async Task<ActionResult<IEnumerable<object>>> GetStats()
        {
            var exceptionsStats = await _exceptionEntityService.GetStatsAsync();

            if (exceptionsStats == null)
                throw new NotFoundException();

            return Ok(exceptionsStats);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExceptionEntity>> GetByIdAsync(int id)
        {
            var exception = await _exceptionEntityService.GetByIdAsync(id);

            if (exception == null)
                throw new NotFoundException();

            return Ok(exception);

        }

        [HttpGet("Today")]
        public async Task<ActionResult<IEnumerable<ExceptionEntity>>> GetToday()
        {
            var exceptions = await _exceptionEntityService.GetTodayAsync();

            if (exceptions == null)
                throw new NotFoundException();

            return Ok(exceptions);

        }

        [HttpGet("Stats/Today")]
        public async Task<ActionResult<IEnumerable<object>>> GetTodayStats()
        {
            var exceptionsStats = await _exceptionEntityService.GetTodayStatsAsync();

            if (exceptionsStats  == null)
                throw new NotFoundException();

   

            return Ok(exceptionsStats);

        }
    }
}
