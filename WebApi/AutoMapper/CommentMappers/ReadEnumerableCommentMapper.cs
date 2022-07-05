using Core.Entities;
using Core.ViewModels.CommentViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.CommentMappers;

public class ReadEnumerableCommentMapper : IEnumerableViewModelMapper<IEnumerable<Comment>, IEnumerable<ReadCommentViewModel>>
{
    private readonly IViewModelMapper<Comment, ReadCommentViewModel> _readMapper;

    public ReadEnumerableCommentMapper(IViewModelMapper<Comment, ReadCommentViewModel> readMapper)
    {
        _readMapper = readMapper;
    }
    public IEnumerable<ReadCommentViewModel> Map(IEnumerable<Comment> source)
    {
        var readViewModels = source.Select(pr => _readMapper.Map(pr));
        return readViewModels;  
    }
}