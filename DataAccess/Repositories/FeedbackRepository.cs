using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private IQueryable<Feedback> GetQuery(
          Expression<Func<Feedback, bool>>? filter = null,
          Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>>? orderBy = null,
          Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>? includeProperties = null,
          bool asNoTracking = false)
        {
            IQueryable<Feedback> feedbackQuery = (
                filter is null
                ? DbSet
                : DbSet.Where(filter)
            );

            if (includeProperties is not null)
                feedbackQuery = includeProperties(feedbackQuery);

            if (orderBy is not null)
                feedbackQuery = orderBy(feedbackQuery);

            if (asNoTracking)
                feedbackQuery = feedbackQuery.AsNoTracking();

            return feedbackQuery;
        }

        public FeedbackRepository(ClinicContext clinicContext) : base(clinicContext)
        {
        }

        public async Task<PagedList<Feedback>> GetPaged(
            FeedbackParameters parameters,
            Expression<Func<Feedback, bool>>? filter = null,
            Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>>? orderBy = null,
            Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>? includeProperties = null)
        {
            var feedbacks = await GetQuery(
                filter: filter,
                orderBy: orderBy,
                includeProperties: includeProperties,
                asNoTracking:true).ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            return feedbacks;
        }
    }
}
