using Core.Entities;
using Core.ViewModel.MessageViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.MessageMappers;

public class MessageGetEnumerableMapper
    : IEnumerableViewModelMapper<IEnumerable<Message>, IEnumerable<MessageGetViewModel>>
{
    private readonly IViewModelMapper<Message, MessageGetViewModel> _mapper;

    public MessageGetEnumerableMapper(IViewModelMapper<Message, MessageGetViewModel> mapper)
    {
        _mapper = mapper;
    }
    
    public IEnumerable<MessageGetViewModel> Map(IEnumerable<Message> source)
    {
        return source.Select(m => _mapper.Map(m));
    }
}