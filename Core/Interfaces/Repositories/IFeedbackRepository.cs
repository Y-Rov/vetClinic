using System.Linq.Expressions;
using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Interfaces.Repositories
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        public Task<PagedList<Feedback>> GetPaged(
            FeedbackParameters parameters,
            Expression<Func<Feedback, bool>>? filter = null,
            Func<IQueryable<Feedback>, IOrderedQueryable<Feedback>>? orderBy = null,
            Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>>? includeProperties = null);
    }
}
