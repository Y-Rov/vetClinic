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
        readonly IFeedbackService _service;

        readonly IViewModelMapper<Feedback, FeedbackReadViewModel> _feedbackMapper;
        readonly IViewModelMapper<FeedbackCreateViewModel, Feedback> _createFeedbackMapper;
        readonly IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> _listMapper;

        public FeedbackController(
            IFeedbackService service, 
            IViewModelMapper<Feedback, FeedbackReadViewModel> feedbackMapper, 
            IViewModelMapper<FeedbackCreateViewModel, Feedback> createFeedbackMapper,
            IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> listMapper)
        {
            _service = service;
            _feedbackMapper = feedbackMapper;
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
        public async Task AddFeedback(FeedbackCreateViewModel feedback)
        {
            var mappedFeedback = 
                _createFeedbackMapper.Map(feedback);

            await _service.AddFeedback(mappedFeedback);

            Ok();
        }
    }
}
