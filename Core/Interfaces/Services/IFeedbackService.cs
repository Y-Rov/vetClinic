using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Services
{
    public interface IFeedbackService
    {
        Task<PagedList<Feedback>> GetAllFeedbacks(FeedbackParameters parameters);
        Task AddFeedback(Feedback feedback);
    }
}
