using Core.Entities;
using Core.ViewModels.CommentViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.CommentMappers;

public class UpdateCommentMapper : IViewModelMapper<UpdateCommentViewModel, Comment>
{
    public Comment Map(UpdateCommentViewModel source)
    {
        return new Comment()
        {
            Content = source.Content,
            Id = source.Id,
            Edited = true
        };
    }
}