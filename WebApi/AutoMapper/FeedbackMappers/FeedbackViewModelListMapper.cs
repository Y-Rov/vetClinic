using Core.Entities;
using Core.ViewModels.FeedbackViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.FeedbackMappers
{
    public class FeedbackViewModelListMapper :
        IViewModelMapper<IEnumerable<Feedback>, IEnumerable<FeedbackReadViewModel>>
    {
        readonly IViewModelMapper<Feedback, FeedbackReadViewModel> _feedbackMapper;

        public FeedbackViewModelListMapper(IViewModelMapper<Feedback, FeedbackReadViewModel> feedbackMapper)
        {
            _feedbackMapper = feedbackMapper;
        }

        public IEnumerable<FeedbackReadViewModel> Map(IEnumerable<Feedback> source)
        {
            foreach(var feedback in source)
                yield return _feedbackMapper.Map(feedback);
        }
    }
}
