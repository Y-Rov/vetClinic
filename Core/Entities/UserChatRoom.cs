namespace Core.Entities;

public class UserChatRoom
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;
}