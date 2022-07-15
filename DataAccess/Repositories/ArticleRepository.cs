using System.Linq.Expressions;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ArticleRepository : Repository<Article>, IArticleRepository
{
    private readonly ClinicContext _clinicContext;

    public ArticleRepository(ClinicContext clinicContext) : base(clinicContext)
    {
        _clinicContext = clinicContext;
    }

    public async Task<PagedList<Article>> GetPaged(
        ArticleParameters parameters, 
        Expression<Func<Article, bool>>? filter = null, 
        Func<IQueryable<Article>, IOrderedQueryable<Article>>? orderBy = null,
        string includeProperties = "")
    {
        var articles = await GetQuery(            
            filter: filter,
            orderBy: orderBy,
            orderByDirection: parameters.OrderByDirection,
            includeProperties: includeProperties).ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
        return articles;    
    }

    private IQueryable<Article> GetQuery(
        Expression<Func<Article, bool>>? filter = null,
        Func<IQueryable<Article>, IOrderedQueryable<Article>>? orderBy = null,
        string? orderByDirection = "",
        string includeProperties = "")
    {
        IQueryable<Article> articleQuery = _clinicContext.Articles;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            articleQuery = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(articleQuery, (current, includeProperty)
                    => current.Include(includeProperty));
        }

        if (filter is not null)
        {
            articleQuery = articleQuery.Where(filter);
        }

        if (orderBy is not null)
        {
            articleQuery = orderBy(articleQuery);
            if (!string.IsNullOrEmpty(orderByDirection) && orderByDirection == "desc")
                articleQuery = articleQuery.Reverse();
        }

        return articleQuery;
    }
}