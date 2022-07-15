using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.ViewModels.FeedbackViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers
{
    [Route("api/feedback")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _service;

        private readonly IViewModelMapper<FeedbackCreateViewModel, Feedback> _createFeedbackMapper;
        private readonly IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> _listMapper;

        public FeedbackController(
            IFeedbackService service, 
            IViewModelMapper<FeedbackCreateViewModel, Feedback> createFeedbackMapper,
            IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> listMapper)
        {
            _service = service;
            _createFeedbackMapper = createFeedbackMapper;
            _listMapper = listMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<FeedbackReadViewModel>> GetAllFeedbacks(
            [FromQuery] CollateParameters parameters)
        {
            var feedbacks = 
                    await _service.GetAllFeedbacks(parameters.FilterParam, parameters.TakeCount, parameters.SkipCount);

            var mappedFeedbacks = _listMapper.Map(feedbacks);

            return mappedFeedbacks;
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> AddFeedback(FeedbackCreateViewModel feedback)
        {
            var mappedFeedback = 
                _createFeedbackMapper.Map(feedback);

            await _service.AddFeedback(mappedFeedback);

            return Ok();
        }
    }
}
