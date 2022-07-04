using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IFeedbackService
    {
        public Task<IEnumerable<Feedback>> GetAllFeedbacks(string? filterParam, int? takeCount, int skipCount = 0);
        public Task AddFeedback(Feedback feedback);
    }
}
