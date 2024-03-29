﻿using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;
        private readonly ILoggerManager _logger;

        private Expression<Func<Feedback, bool>>? GetFilterQuery (string? filterParam)
        {
            Expression<Func<Feedback, bool>>? filterQuery = null;
            
            if(filterParam is not null)
            {
                string formatedParameter = filterParam.Trim().ToLower();

                filterQuery = feedback =>
                    feedback.Email.ToLower().Contains(formatedParameter);
            }
            
            return filterQuery;
        }

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

            _logger.LogInfo($"The new feedback with ID = {feedback.Id} was added");
        }

        public async Task<PagedList<Feedback>> GetAllFeedbacks(
            FeedbackParameters parameters)
        {
            _logger.LogInfo("All feedbacks were received");

            return await _repository.GetPaged(
                parameters: parameters,
                filter: GetFilterQuery(parameters.FilterParam),
                includeProperties: query =>
                    query.Include(feedback => feedback.User));
        }
    }
}
