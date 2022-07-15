using Core.Entities;
using Core.Paginator;
using Core.ViewModels;
using Core.ViewModels.ArticleViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ArticleMappers;

public class PagedArticlesMapper : IViewModelMapper<PagedList<Article>, PagedReadViewModel<ReadArticleViewModel>>
{
    private readonly IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>> _enumMapper;

    public PagedArticlesMapper(IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>> enumMapper)
    {
        _enumMapper = enumMapper;
    }

    public PagedReadViewModel<ReadArticleViewModel> Map(PagedList<Article> source)
    {
        return new PagedReadViewModel<ReadArticleViewModel>()
        {
            CurrentPage = source.CurrentPage,
            PageSize = source.PageSize,
            TotalCount = source.TotalCount,
            HasPrevious = source.HasPrevious,
            HasNext = source.HasNext,
            TotalPages = source.TotalPages,
            Entities = _enumMapper.Map(source.AsEnumerable())
        };
    }
}