using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
using Core.ViewModels.ExceptionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/exceptions")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ExceptionController : ControllerBase
    {
        private readonly IExceptionEntityService _exceptionEntityService;
        private readonly IViewModelMapper<PagedList<ExceptionEntity>, PagedReadViewModel<ExceptionEntityReadViewModel>> _exceptionToPagedModel;
        private readonly IViewModelMapper<PagedList<ExceptionStats>, PagedReadViewModel<ExceptionStatsReadViewModel>> _exceptionStatsToPagedModel;
        public ExceptionController(
            IExceptionEntityService exceptionEntityService,
            IViewModelMapper<PagedList<ExceptionEntity>, PagedReadViewModel<ExceptionEntityReadViewModel>> exceptionToPagedModel,
            IViewModelMapper<PagedList<ExceptionStats>, PagedReadViewModel<ExceptionStatsReadViewModel>> exceptionStatsToPagedModel)
        {
            _exceptionEntityService = exceptionEntityService;
            _exceptionToPagedModel = exceptionToPagedModel;
            _exceptionStatsToPagedModel = exceptionStatsToPagedModel;
        }


        [HttpGet]
        public async Task<ActionResult<PagedReadViewModel<ExceptionEntity>>> GetAsync([FromQuery] ExceptionParameters exceptionParameters)
        {
            var allExceptions = await _exceptionEntityService.GetAsync(exceptionParameters);

            var exceptionsReadModel = _exceptionToPagedModel.Map(allExceptions);

            return Ok(exceptionsReadModel);
        }


        [HttpGet("test")]
        public async Task<ActionResult<PagedReadViewModel<ExceptionEntity>>> GetTodddayAsync([FromQuery] ExceptionParameters exceptionParameters)
        {
            var allExceptions = await _exceptionEntityService.GetTodayAsync(exceptionParameters);

            var exceptionsReadModel = _exceptionToPagedModel.Map(allExceptions);
            return Ok(exceptionsReadModel);

        }

        [HttpGet("stats")]
        public async Task<ActionResult<PagedReadViewModel<ExceptionStatsReadViewModel>>> GetStatsAsync([FromQuery] ExceptionParameters exceptionParameters)
        {
            var exceptionsStats = await _exceptionEntityService.GetStatsAsync(exceptionParameters);

            var exceptionStatsReadModel = _exceptionStatsToPagedModel.Map(exceptionsStats);
            return Ok(exceptionStatsReadModel);

        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ExceptionEntity>> GetAsync([FromRoute] int id)
        {
            var exception = await _exceptionEntityService.GetAsync(id);
            return Ok(exception);

        }

        [HttpGet("today")]
        public async Task<ActionResult<PagedReadViewModel<ExceptionEntity>>> GetTodayAsync([FromQuery] ExceptionParameters exceptionParameters)
        {
            var allExceptions = await _exceptionEntityService.GetTodayAsync(exceptionParameters);

            var exceptionsReadModel = _exceptionToPagedModel.Map(allExceptions);
            return Ok(exceptionsReadModel);

        }

        [HttpGet("stats/today")]
        public async Task<ActionResult<PagedReadViewModel<ExceptionStatsReadViewModel>>> GetTodayStatsAsync([FromQuery] ExceptionParameters exceptionParameters)
        {
            var exceptionsStats = await _exceptionEntityService.GetTodayStatsAsync(exceptionParameters);
            var exceptionStatsReadModel = _exceptionStatsToPagedModel.Map(exceptionsStats);
            return Ok(exceptionStatsReadModel);

        }
    }
}
