using Core.Entities;
using Core.ViewModels.CommentViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.CommentMappers;

public class CreateCommentMapper : IViewModelMapper<CreateCommentViewModel, Comment>
{
    public Comment Map(CreateCommentViewModel source)
    {
        return new Comment()
        {
            Content = source.Content,
            AuthorId = source.AuthorId,
            ArticleId = source.ArticleId,
            CreatedAt = DateTime.Now
        };
    }
}