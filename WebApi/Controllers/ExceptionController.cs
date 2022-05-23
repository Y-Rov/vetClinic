using AutoMapper;
using Core.DTO;
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
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        public ExceptionController(IExceptionEntityService exceptionEntityService, ILoggerManager loggerManager, IMapper mapper)
        {
            _exceptionEntityService = exceptionEntityService;
            _loggerManager = loggerManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExceptionEntityReadDto>>> GetAllAsync()
        {
            _loggerManager.LogInfo("Open ExceptionController action GetAllAsync()");
            var allExceptions = await _exceptionEntityService.GetAllAsync();

            if (allExceptions == null)
            {
                _loggerManager.LogError("Error (NotFoundException) at ExceptionController action GetAllAsync()");
                throw new NotFoundException();
            }

            _loggerManager.LogInfo("Exit ExceptionController action GetAllAsync()");
            var readDtos = _mapper.Map<IEnumerable<ExceptionEntityReadDto>>(allExceptions);
            return Ok(readDtos);
        }

        [HttpGet("Stats")]
        public async Task<ActionResult<IEnumerable<object>>> GetStatsAsync()
        {
            _loggerManager.LogInfo("Open ExceptionController action GetStatsAsync()");
            var exceptionsStats = await _exceptionEntityService.GetStatsAsync();

            if (exceptionsStats == null)
            {
                _loggerManager.LogError("Error (NotFoundException) at ExceptionController action GetStatsAsync()");
                throw new NotFoundException();
            }

            _loggerManager.LogInfo("Exit ExceptionController action GetStatsAsync()");
            return Ok(exceptionsStats);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExceptionEntity>> GetByIdAsync(int id)
        {
            _loggerManager.LogInfo("Open ExceptionController action GetByIdAsync()");
            var exception = await _exceptionEntityService.GetByIdAsync(id);

            if (exception == null)
            {
                _loggerManager.LogError("Error (NotFoundException) at ExceptionController action GetByIdAsync()");
                throw new NotFoundException();
            }

            _loggerManager.LogInfo("Exit ExceptionController action GetByIdAsync()");
            return Ok(exception);

        }

        [HttpGet("Today")]
        public async Task<ActionResult<IEnumerable<ExceptionEntityReadDto>>> GetTodayAsync()
        {
            _loggerManager.LogInfo("Open ExceptionController action GetTodayAsync()");
            var allExceptions = await _exceptionEntityService.GetTodayAsync();

            if (allExceptions == null)
            {
                _loggerManager.LogError("Error (NotFoundException) at ExceptionController action GetTodayAsync()");
                throw new NotFoundException();
            }

            _loggerManager.LogInfo("Exit ExceptionController action GetTodayAsync()");
            var readDtos = _mapper.Map<IEnumerable<ExceptionEntityReadDto>>(allExceptions);
            return Ok(readDtos);

        }

        [HttpGet("Stats/Today")]
        public async Task<ActionResult<IEnumerable<object>>> GetTodayStatsAsync()
        {
            _loggerManager.LogInfo("Open ExceptionController action GetTodayStatsAsync()");
            var exceptionsStats = await _exceptionEntityService.GetTodayStatsAsync();

            if (exceptionsStats == null)
            {
                _loggerManager.LogError("Error (NotFoundException) at ExceptionController action GetTodayStatsAsync()");
                throw new NotFoundException();
            }

            _loggerManager.LogInfo("Exit ExceptionController action GetTodayStatsAsync()");
            return Ok(exceptionsStats);

        }
    }
}
