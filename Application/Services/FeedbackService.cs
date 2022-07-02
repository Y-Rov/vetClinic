using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        readonly IFeedbackRepository _repository;
        readonly ILoggerManager _logger;

        public FeedbackService(
            IFeedbackRepository repository, 
            ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task AddFeedback(Feedback feedback)
        {
            await _repository.InsertAsync(feedback);

            await _repository.SaveChangesAsync();

            _logger.LogInfo($"feedback was added");
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacks()
        {
            _logger.LogInfo($"feedbacks were recieved");

            return await _repository.QueryAsync(
                asNoTracking: true,
                include: query =>
                    query.Include(feedback => feedback.User));
        }
    }
}
