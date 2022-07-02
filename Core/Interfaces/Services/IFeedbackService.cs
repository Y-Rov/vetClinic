using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IFeedbackService
    {
        public Task<IEnumerable<Feedback>> GetAllFeedbacks();
        public Task AddFeedback(Feedback feedback);
    }
}
