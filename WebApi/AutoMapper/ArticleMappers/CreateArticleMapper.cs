using Core.Entities;
using Core.ViewModels.ArticleViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ArticleMappers;

public class CreateArticleMapper : IViewModelMapper<CreateArticleViewModel, Article>
{
    public Article Map(CreateArticleViewModel source)
    {
        return new Article()
        {
            Title =  source.Title,
            Body = source.Body,
            AuthorId = source.AuthorId,
            CreatedAt = DateTime.Now,
            Published = source.Published,
        };
    }
}