using Core.Emuns;

namespace Core.Entities;

public class ChatRoom
{
    public int Id { get; set; }
    public ChatType Type { get; set; }
    public IEnumerable<UserChatRoom> UserChatRooms { get; set; } = new List<UserChatRoom>();
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
}