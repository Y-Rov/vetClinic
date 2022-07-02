using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.FeedbackViewModels;
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
        public async Task<IEnumerable<FeedbackReadViewModel>> GetAllFeedbacks()
        {
            var feedbacks = await _service.GetAllFeedbacks();

            var mappedFeedbacks = _listMapper.Map(feedbacks);

            return mappedFeedbacks; 
        }

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
