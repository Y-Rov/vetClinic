using Core.Entities;
using Core.Interfaces.Services;
using Core.Models;
using Core.Paginator;
using Core.Paginator.Parameters;
using Core.ViewModels;
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
        private readonly IViewModelMapper<PagedList<Feedback>, PagedReadViewModel<FeedbackReadViewModel>> _pagedListMapper;

        public FeedbackController(
            IFeedbackService service, 
            IViewModelMapper<FeedbackCreateViewModel, Feedback> createFeedbackMapper,
            IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>> listMapper,
            IViewModelMapper<PagedList<Feedback>, PagedReadViewModel<FeedbackReadViewModel>> pagedListMapper)
        {
            _service = service;
            _createFeedbackMapper = createFeedbackMapper;
            _pagedListMapper = pagedListMapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PagedReadViewModel<FeedbackReadViewModel>> GetAllFeedbacks(
            [FromQuery] FeedbackParameters parameters)
        {
            var feedbacks = 
                    await _service.GetAllFeedbacks(parameters);

            var mappedFeedbacks = _pagedListMapper.Map(feedbacks);

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
