using Core.Entities;
using Core.ViewModels.ArticleViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ArticleMappers;

public class UpdateArticleMapper : IViewModelMapper<UpdateArticleViewModel, Article>
{
    public Article Map(UpdateArticleViewModel source)
    {
        return new Article()
        {
            Id = source.Id,
            Edited = true,
            Title = source.Title,
            Body = source.Body,
            AuthorId = source.AuthorId,
            CreatedAt = source.CreatedAt,
            Published = source.Published,
        };
    }
}