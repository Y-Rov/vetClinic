using Core.Entities;
using Core.ViewModels.ChatRoomViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ChatMappers;

public class ChatRoomGetEnumerableMapper : 
    IUserOrientedEnumerableViewModelMapper<ChatRoom, ChatRoomGetViewModel>
{
    private readonly IUserOrientedViewModelMapper<ChatRoom, ChatRoomGetViewModel> _getMapper;

    public ChatRoomGetEnumerableMapper(IUserOrientedViewModelMapper<ChatRoom, ChatRoomGetViewModel> getMapper)
    {
        _getMapper = getMapper;
    }
    
    public IEnumerable<ChatRoomGetViewModel> Map(IEnumerable<ChatRoom> source, int userId)
    {
        return source.Select(c => _getMapper.Map(c, userId));
    }
}