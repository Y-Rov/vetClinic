using Core.Emuns;
using Core.Entities;
using Core.ViewModels.ChatRoomViewModels;
using WebApi.AutoMapper.Interface;

namespace WebApi.AutoMapper.ChatMappers;

public class ChatRoomGetMapper : IUserOrientedViewModelMapper<ChatRoom, ChatRoomGetViewModel>
{
    public ChatRoomGetViewModel Map(ChatRoom source, int userId)
    {
        var map = new ChatRoomGetViewModel()
        {
            Id = source.Id,
            Type = ChatType.Private
        };
        var user = source.UserChatRooms.Select(ur => ur.User).FirstOrDefault(u => u.Id != userId);

        if (user is not null)
        {
            map.Name = $"{user.FirstName} {user.LastName}";
            map.InterlocutorId = user.Id;
            map.Picture = user.ProfilePicture;
        }

        return map;
    }
}