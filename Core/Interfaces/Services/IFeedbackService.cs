using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface IFeedbackService
    {
        public Task<PagedList<Feedback>> GetAllFeedbacks(FeedbackParameters parameters);
        public Task AddFeedback(Feedback feedback);
    }
}
