using Core.Entities;
using Core.ViewModels.ArticleViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ArticleMappers;

public class ReadArticleMapper : IViewModelMapper<Article, ReadArticleViewModel>
{
    public ReadArticleViewModel Map(Article source)
    {
        return new ReadArticleViewModel()
        {
            Id = source.Id,
            Edited = source.Edited,
            Title = source.Title,
            Body = source.Body,
            AuthorId = source.AuthorId,
            CreatedAt = source.CreatedAt,
            Published = source.Published,
            AuthorName = source.Author!.FirstName + " " + source.Author!.LastName
        };
    }
}