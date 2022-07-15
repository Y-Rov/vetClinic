using System.Linq.Expressions;
using Core.Entities;
using Core.Paginator;
using Core.Paginator.Parameters;

namespace Core.Interfaces.Repositories;

public interface IArticleRepository : IRepository<Article>
{
    Task<PagedList<Article>> GetPaged(
        ArticleParameters parameters,
        Expression<Func<Article, bool>>? filter = null,
        Func<IQueryable<Article>, IOrderedQueryable<Article>>? orderBy = null,
        string includeProperties = "");
}