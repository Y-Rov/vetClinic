using Core.Entities;
using Core.ViewModels.ArticleViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ArticleMappers;

public class ReadEnumerableArticleMapper : IEnumerableViewModelMapper<IEnumerable<Article>, IEnumerable<ReadArticleViewModel>>
{
    private readonly IViewModelMapper<Article, ReadArticleViewModel> _readMapper;

    public ReadEnumerableArticleMapper(IViewModelMapper<Article, ReadArticleViewModel> readMapper)
    {
        _readMapper = readMapper;
    }
    public IEnumerable<ReadArticleViewModel> Map(IEnumerable<Article> source)
    {
        var readViewModels = source.Select(pr => _readMapper.Map(pr));
        return readViewModels;    
    }
}